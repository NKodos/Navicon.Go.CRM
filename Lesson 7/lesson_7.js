var Navicon = Navicon || {};
Navicon.lesson_7 = Navicon.lesson_7 || {};

// Task 1
// Объект Договор: после выбора значения в поле кредитная программа, 
// проверять ее срок действия относительно даты договора. Если срок истек, 
// система должна показывать пользователю сообщение и блокировать 
// сохранение формы.

Navicon.lesson_7.task1 = (function () {
    const alertDelay = 5000;

    const creditIdOnChange = function (context) {
        isDateValid(context, () => {
            showNotValidDatesNotification(3);
        });
    };

    const entityOnSave = function (context) {
        isDateValid(context, () => {
            showNotValidDatesNotification(2);
            context.getEventArgs().preventDefault();
        });
    };

    const isDateValid = function (context, failFunction) {
        const formContext = context.getFormContext();
        const creditIdAttr = formContext.getAttribute('new_creditid');
        if (creditIdAttr == null) return;

        const creditIdValue = creditIdAttr.getValue();
        console.log("creditid =", creditIdValue);
        if (creditIdValue == null || creditIdValue.length < 1) return;

        const creditId = creditIdValue[0].id;
        const creditPromise = Xrm.WebApi.retrieveRecord('new_credit', creditId, '?$select=new_dateend');

        creditPromise.then(
            function (entity) {
                const dateAttr = formContext.getAttribute('new_date');
                if (dateAttr == null) return;
                const dateValue = dateAttr.getValue();

                const dateEnd = new Date(entity.new_dateend);
                if (!isFirstDateGreaterThanSecondDate(dateEnd, dateValue)) {
                    failFunction();
                }
            },
            function (error) {
                console.log('Попытка валидации даты кредита: ' + error.message);
            }
        );
    };

    const isFirstDateGreaterThanSecondDate = function (firstDate, secondDate) {
        return firstDate.getTime() > secondDate.getTime();
    };

    const showNotValidDatesNotification = function (level) {
        const notification =
        {
            type: 2,
            level: level,
            showCloseButton: true,
            message: "Выбранная кредитная программа просрочена. " +
                "Дата договора должна быть меньше даты окончания кредитной программы."
        };

        Xrm.App.addGlobalNotification(notification).then(
            function success(result) {
                console.log("Notification created with ID: " + result);
                window.setTimeout(function () {
                    Xrm.App.clearGlobalNotification(result);
                }, alertDelay);
            },
            function (error) {
                console.log('Отображение уведомления: ' + error.message);
            }
        );
    };

    return {
        onLoad: function (context) {

            let formContext = context.getFormContext();
            let creditIdAttr = formContext.getAttribute('new_creditid');

            creditIdAttr.addOnChange(creditIdOnChange);
            formContext.data.entity.addOnSave(entityOnSave);
        }
    };
})();

// Task 2
// 2.	Объект Договор: после выбора значения в поле кредитная программа, 
// срок кредита должен подставляться из выбранной кредитной программы в договор.

Navicon.lesson_7.task2 = (function () {
    const alertDelay = 5000;

    const creditIdOnChange = function (context) {
        const formContext = context.getFormContext();
        const creditIdAttr = formContext.getAttribute('new_creditid');
        const creditPeriodAttr = formContext.getAttribute('new_creditperiod');

        if (creditIdAttr == null) return;
        const creditIdValue = creditIdAttr.getValue();
        console.log("creditid =", creditIdValue);

        if (creditIdValue == null || creditIdValue.length < 1) {
            creditPeriodAttr.setValue(null);
            return;
        }

        const creditId = creditIdValue[0].id;
        const creditPromise = Xrm.WebApi.retrieveRecord('new_credit', creditId,
            '?$select=new_datestart, new_dateend');

        creditPromise.then(
            function (entity) {

                const dateStart = new Date(entity.new_datestart);
                const dateEnd = new Date(entity.new_dateend);

                const periodValue = getCreditPeriod(dateEnd, dateStart);
                creditPeriodAttr.setValue(periodValue);
            },
            function (error) {
                console.log('При установки периода кредита: ' + error.message);
            }
        );
    };

    const getCreditPeriod = function (firstDate, secondDate) {
        return firstDate.getFullYear() - secondDate.getFullYear();
    };

    return {
        onLoad: function (context) {

            const formContext = context.getFormContext();
            const creditIdAttr = formContext.getAttribute('new_creditid');
            creditIdAttr.addOnChange(creditIdOnChange);
            creditIdOnChange(context);
        }
    };
})();

// Task 3
// При выборе объекта Автомобиль в объекте Договор, стоимость должна подставляться 
// Автоматически в соответствии с правилом:
// - Если автомобиль с пробегом, стоимость берется из поля Сумма на объекте Автомобиль
// - Если автомобиль без пробега, стоимость берется из поля сумма объекта Модель, указанной на Автомобиле.

Navicon.lesson_7.task3 = (function () {
    const alertDelay = 5000;

    const autoIdOnChange = function (context) {
        const formContext = context.getFormContext();
        const autoIdAttr = formContext.getAttribute('new_autoid');
        const summaAttr = formContext.getAttribute('new_summa');

        if (autoIdAttr == null) return;

        const autoIdValue = autoIdAttr.getValue();
        if (autoIdValue == null || autoIdValue.length < 1) {
            summaAttr.setValue(null);
            return;
        }

        const autoId = autoIdValue[0].id;
        const autoPromise = Xrm.WebApi.retrieveRecord('new_auto', autoId,
            '?$select=new_used,new_amount&$expand=new_modelid($select=new_recommendedamount)');

        autoPromise.then(
            function (autoEntity) {
                if (autoEntity == null) return;
                if (autoEntity.new_used) {
                    summaAttr.setValue(autoEntity.new_amount);
                } else {
                    if (autoEntity.new_modelid != null) {
                        summaAttr.setValue(autoEntity.new_modelid.new_recommendedamount);
                    }
                }
            },
            function (error) {
                console.log('При расчете стоимости договора: ' + error.message);
            }
        );
    };

    return {
        onLoad: function (context) {
            const formContext = context.getFormContext();
            const autoIdAttr = formContext.getAttribute('new_autoid');
            if (autoIdAttr != null) {
                autoIdAttr.addOnChange(autoIdOnChange);
            }
        }
    };
})();