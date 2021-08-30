var Navicon = Navicon || {};
Navicon.nav_agreement = Navicon.nav_agreement || {};

// Task 1
// При создании объекта Договор, сразу после открытия карточки доступны для редактирования 
// поля: номер, дата договора, контакт и модель. Остальные поля - скрыты. 
// Вкладка с данными по кредиту скрыта.

Navicon.nav_agreement.task1 = (function () {
    const creatingFormType = 1;
    const enabledControlsForCreating = ['new_number', 'new_date',
        'new_contact', 'new_autoid'];

    const disableCreationFormAttributes = function (context) {
        const formContext = context.getFormContext();

        enabledControlsForCreating.forEach(controlName => {
            const control = formContext.getControl(controlName);
            control.setDisabled(true);
        });

        const creditTab = formContext.ui.tabs.get('Credit');
        if (creditTab != null) {
            creditTab.setVisible(false);
        }
    };

    return {
        onLoad: function (context) {

            const formContext = context.getFormContext();
            const formType = formContext.ui.getFormType();

            if (formType == creatingFormType) {
                disableCreationFormAttributes(context);
            }
        }
    };
})();

// Task 2
// На объекте Договор, после выбора значения в полях контакт и автомобиль, 
// становиться доступной вкладка кредитная программа.

Navicon.nav_agreement.task2 = (function () {

    const autoAndContactOnChange = function (context) {
        const formContext = context.getFormContext();

        const contactAttrValue = formContext.getAttribute('new_contact').getValue();
        const autoidAttrValue = formContext.getAttribute('new_autoid').getValue();

        const creditTab = formContext.ui.tabs.get('Credit');
        const creditidControl = formContext.getControl('new_creditid');

        if (contactAttrValue && autoidAttrValue) {
            creditTab.setVisible(true);
            creditidControl.setDisabled(false);
        } else {
            creditTab.setVisible(false);
            creditidControl.setDisabled(true);
        }
    };

    return {
        onLoad: function (context) {

            const formContext = context.getFormContext();

            const contactAttr = formContext.getAttribute('new_contact');
            const autoidAttr = formContext.getAttribute('new_autoid');

            if (contactAttr) {
                contactAttr.addOnChange(autoAndContactOnChange);
            }
            if (autoidAttr) {
                autoidAttr.addOnChange(autoAndContactOnChange);
            }
        }
    };
})();

// Task 3
// На объекте Договор, после выбора кредитной программы, становятся доступными для 
// редактирования поля, связанные с расчетом кредита. 

Navicon.nav_agreement.task3 = (function () {

    const creditidOnChange = function (context) {

        const formContext = context.getFormContext();
        const creditAttr = formContext.getAttribute('new_creditid');
        const creditValue = creditAttr.getValue();

        console.log(creditValue);

        if (creditValue) {
            disableCreditControls(context, false);
        } else {
            disableCreditControls(context, true);
        }
    };

    const disableCreditControls = function (context, isDisable) {

        const formContext = context.getFormContext();
        const creditAttributeNames = ['new_creditperiod', 'new_creditamount',
        'new_fullcreditamount', 'new_initialfee', 'new_factsumma',
        'new_paymentplandate'];

        creditAttributeNames.forEach(controlName => {
            const control = formContext.getControl(controlName);

            if (control) {
                control.setDisabled(isDisable);
            }
        });
    }

    return {
        onLoad: function (context) {

            const formContext = context.getFormContext();
            const creditAttr = formContext.getAttribute('new_creditid');

            creditAttr.addOnChange(creditidOnChange);
            creditidOnChange(context);
        }
    };
})();

// Task 4
// На объекте Договор, поскольку кредитные программы связаны с объектом 
// Автомобиль отношением N:N то в договоре при выборе кредитной программы 
// в списке лукап поля должны быть доступны только программы, связанные  
// с выбранным объектом Автомобиль.

Navicon.nav_agreement.task4 = (function () {

    const filterCredit = function (context) {
        const formContext = context.getFormContext();
        const autoidAttr = formContext.getAttribute('new_autoid');
        const autoidValue = autoidAttr.getValue();

        if (autoidValue == null || autoidValue.length < 1 || autoidValue[0] == null) return;

        const relationshipPromise = Xrm.WebApi.retrieveMultipleRecords('new_new_credit_new_auto', '?$filter=new_autoid eq ' +
            autoidValue[0].id);

        let filter = "<filter type='and'><condition attribute='new_creditid' operator='in'>";
        const control = formContext.getControl('new_creditid');

        relationshipPromise.then(
            function (entityResult) {
                console.log(entityResult);

                for (let i = 0; i < entityResult.entities.length; i++) {
                    filter += `<value> ${entityResult.entities[i].new_creditid} </value>`;
                }
                filter += "</condition></filter>";

                control.addPreSearch(context => {
                    console.log("filter=", filter);
                    control.addCustomFilter(filter);
                });

            },
            function (error) {
                console.error(`При добавлении фильтра в поле new_creditid: ${error.message}`);
            }
        );
    };

    return {
        onLoad: function (context) {

            filterCredit(context);
        }
    };
})();

// Task 5
// На объекте Кредитная программа необходимо проверять, чтобы дата 
// окончания была больше даты начала, не менее, чем на год. В случае 
// невыполнения условия, показывать информационное сообщение и блокировать 
// сохранение формы.

Navicon.nav_agreement.task5 = (function () {
    const alertDelay = 5000;

    const dateStartOnChange = function (context) {
        const datesIsNotValid = !isDateEndOneYearLaterThanDateStart(context);

        if (datesIsNotValid) {
            showNotValidDatesNotification(3);
        }
    };

    const entityOnSave = function (context) {
        const datesIsNotValid = !isDateEndOneYearLaterThanDateStart(context);

        if (datesIsNotValid) {
            context.getEventArgs().preventDefault();
            showNotValidDatesNotification(2);
        }
    };

    const isDateEndOneYearLaterThanDateStart = function (context) {
        const formContext = context.getFormContext();
        const dateStartAttr = formContext.getAttribute('new_datestart');
        const dateEndAttr = formContext.getAttribute('new_dateend');
        const dateStartValue = dateStartAttr.getValue();
        const dateEndValue = dateEndAttr.getValue();
        console.log(`dateStart=${dateStartValue}\n
        dateEnd=${dateEndValue}`);

        const dateDiff = dateEndValue.getFullYear() - dateStartValue.getFullYear();
        console.log(`dateDiff=${dateDiff}`);
        return dateDiff >= 1;
    };

    const showNotValidDatesNotification = function (level) {
        const notification =
        {
            type: 2,
            level: level,
            showCloseButton: true,
            message: "Дата окончания должна быть больше даты начала, не менее, чем на год"
        };

        Xrm.App.addGlobalNotification(notification).then(
            function success(result) {
                console.log("Notification created with ID: " + result);
                window.setTimeout(function () {
                    Xrm.App.clearGlobalNotification(result);
                }, alertDelay);
            },
            function (error) {
                console.log('При отображении уведомления: ' + error.message);
            }
        );
    };

    return {
        onLoad: function (context) {
            const formContext = context.getFormContext();
            const dateStartAttr = formContext.getAttribute('new_datestart');
            const dateEndAttr = formContext.getAttribute('new_dateend');

            dateStartAttr.addOnChange(dateStartOnChange);
            dateEndAttr.addOnChange(dateStartOnChange);
            formContext.data.entity.addOnSave(entityOnSave);
        }
    };
})();

// Task 6
// На объекте Договор, реализовать функцию для поля номер договора - 
// по завершении ввода, оставлять только цифры и тире.

Navicon.nav_agreement.task6 = (function () {

    const numberOnChange = function (context) {
        const formContext = context.getFormContext();
        const numberAttr = formContext.getAttribute('new_number');
        const numberValue = numberAttr.getValue();

        if (numberValue == null) return;
        const resultNumber = numberValue.replace(/[^\d\-]/g, '');
        numberAttr.setValue(resultNumber);
    };

    return {
        onLoad: function (context) {
            const formContext = context.getFormContext();
            const numberAttr = formContext.getAttribute('new_number');

            numberAttr.addOnChange(numberOnChange);
        }
    };
})();

// Task 7
// На форме объекта Средство связи, при создании поля Телефон и Email скрыты. 
// При выборе пользователем значения в поле Тип, необходимо отображать соответствующее поле: 
// Если тип = Телефон, отображать поле Телефон
// Если тип = E-mail, отображать поле Email.  


Navicon.nav_agreement.task7 = (function () {

    const typeOnChange = function (context) {
        const formContext = context.getFormContext();
        const typeAttr = formContext.getAttribute('new_type');
        setVisibleControlsByType(context, typeAttr.getValue());
    };

    const setVisibleControlsByType = function (context, typeValue) {
        const formContext = context.getFormContext();
        const phoneControl = formContext.getControl('new_phone');
        const emailControl = formContext.getControl('new_email');

        switch (typeValue) {
            // Телефон
            case 100000000:
                phoneControl.setVisible(true);
                emailControl.setVisible(false);
                break;
            // E-mail
            case 100000001:
                phoneControl.setVisible(false);
                emailControl.setVisible(true);
                break;
            default:
                phoneControl.setVisible(false);
                emailControl.setVisible(false);
                break;
        }
    };

    return {
        onLoad: function (context) {
            const formContext = context.getFormContext();
            const typeAttr = formContext.getAttribute('new_type');
            let option = typeAttr.getSelectedOption();

            option = option || { text: '' };
            setVisibleControlsByType(context, option.text);

            typeAttr.addOnChange(typeOnChange);
        }
    };
})();

// Task 8
// Поля на объекте Автомобиль new_auto: Пробег, Количество владельцев, 
// был в ДТП отображаются только при значении в поле С пробегом(new_used)=true. 


Navicon.nav_agreement.task8 = (function () {

    const usedChange = function (context) {
        const formContext = context.getFormContext();
        const usedAttr = formContext.getAttribute('new_used');

        const value = usedAttr.getValue();
        setVisibleControlsByType(context, value);
    };

    const setVisibleControlsByType = function (context, boolValue) {
        if (boolValue == null) return;

        const formContext = context.getFormContext();
        const kmControl = formContext.getControl('new_km');
        const ownerscountControl = formContext.getControl('new_ownerscount');
        const isdamagedControl = formContext.getControl('new_isdamaged');

        kmControl.setVisible(boolValue);
        ownerscountControl.setVisible(boolValue);
        isdamagedControl.setVisible(boolValue);
    };

    return {
        onLoad: function (context) {
            const formContext = context.getFormContext();
            const usedAttr = formContext.getAttribute('new_used');

            const value = usedAttr.getValue();
            setVisibleControlsByType(context, value);

            usedAttr.addOnChange(usedChange);
        }
    };
})();