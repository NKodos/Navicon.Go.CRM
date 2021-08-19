var Navicon = Navicon || {};
Navicon.lesson_7 = Navicon.lesson_7 || {};

// Task 1
// Объект Договор: после выбора значения в поле кредитная программа, 
// проверять ее срок действия относительно даты договора. Если срок истек, 
// система должна показывать пользователю сообщение и блокировать 
// сохранение формы.

Navicon.lesson_7.task1 = (function () {
    const alertDelay = 5000;

    var creditIdOnChange = function (context) {
        let isDateNotValid = !isDateValid(context);

        if (isDateNotValid) {
            showNotValidDatesNotification(3);
        }
    };

    var entityOnSave = function (context) {
        let isDateNotValid = !isDateValid(context);

        if (isDateNotValid) {
            showNotValidDatesNotification(2);
            context.getEventArgs().preventDefault();
        }
    };

    var isDateValid = function (context) {
        let formContext = context.getFormContext();
        let creditIdAttr = formContext.getAttribute('new_creditid');
        if (creditIdAttr == null) return;

        let creditIdValue = creditIdAttr.getValue();
        console.log("creditid =", creditIdValue);
        if (creditIdValue == null || creditIdValue.length < 1) return;

        let creditId = creditIdValue[0].id;
        let creditPromise = Xrm.WebApi.retrieveRecord('new_credit', creditId, '?$select=new_dateend');

        creditPromise.then(
            function (entity) {
                let dateAttr = formContext.getAttribute('new_date');
                if (dateAttr == null) return;
                let dateValue = dateAttr.getValue();

                var dateEnd = new Date(entity.new_dateend);
                return isFirstDateGreaterThanSecondDate(dateEnd, dateValue);
            },
            function (error) {
                console.log(error.message);
            }
        );
    };

    var isFirstDateGreaterThanSecondDate = function (firstDate, secondDate) {
        return firstDate.getTime() > secondDate.getTime();
    };

    var showNotValidDatesNotification = function (level) {
        let notification =
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
                console.log(error.message);
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

    var creditIdOnChange = function (context) {
        let formContext = context.getFormContext();
        let creditIdAttr = formContext.getAttribute('new_creditid');
        let creditPeriodAttr = formContext.getAttribute('new_creditperiod');
        if (creditIdAttr == null) return;
        let creditIdValue = creditIdAttr.getValue();
        console.log("creditid =", creditIdValue);
        
        if (creditIdValue == null || creditIdValue.length < 1) {
            creditPeriodAttr.setValue("");
            return;
        }

        let creditId = creditIdValue[0].id;
        let creditPromise = Xrm.WebApi.retrieveRecord('new_credit', creditId, 
        '?$select=new_datestart, new_dateend');

        creditPromise.then(
            function (entity) {

                var dateStart = new Date(entity.new_datestart);
                var dateEnd = new Date(entity.new_dateend);

                var periodValue = GetCreditPeriod(dateEnd, dateStart);
                creditPeriodAttr.setValue(periodValue);
            },
            function (error) {
                console.log(error.message);
            }
        );
    };

    var GetCreditPeriod = function (firstDate, secondDate) {
        return firstDate.getFullYear() - secondDate.getFullYear();
    };

    return {
        onLoad: function (context) {

            let formContext = context.getFormContext();
            let creditIdAttr = formContext.getAttribute('new_creditid');
            creditIdAttr.addOnChange(creditIdOnChange);
        }
    };
})();