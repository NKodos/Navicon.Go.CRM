var Navicon = Navicon || {};
Navicon.nav_agreement = Navicon.nav_agreement || {};
const creatingFormType = 1;
const enabledControlsForCreating = ['new_number', 'new_date',
    'new_contact', 'new_autoid'];
const creditAttributeNames = ['new_creditperiod', 'new_creditamount',
    'new_fullcreditamount', 'new_initialfee', 'new_factsumma',
    'new_paymentplandate'];

// Task 1
// При создании объекта Договор, сразу после открытия карточки доступны для редактирования 
// поля: номер, дата договора, контакт и модель. Остальные поля - скрыты. 
// Вкладка с данными по кредиту скрыта.

Navicon.nav_agreement.task1 = (function () {

    var disableCreationFormAttributes = function (context) {
        let formContext = context.getFormContext();
        let controls = formContext.getControl();

        controls.forEach(control => {
            let controlName = control.getName();
            if (!enabledControlsForCreating.includes(controlName)) {
                control.setDisabled(true);
            }
        });

        let creditTab = formContext.ui.tabs.get('Credit');
        creditTab.setVisible(false);
    };

    return {
        onLoad: function (context) {

            let formContext = context.getFormContext();
            let formType = formContext.ui.getFormType();

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

    var autoAndContactOnChange = function (context) {
        let formContext = context.getFormContext();

        let contactAttrValue = formContext.getAttribute('new_contact').getValue();
        let autoidAttrValue = formContext.getAttribute('new_autoid').getValue();

        let creditTab = formContext.ui.tabs.get('Credit');
        let creditidControl = formContext.getControl('new_creditid');

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

            let formContext = context.getFormContext();

            let contactAttr = formContext.getAttribute('new_contact');
            let autoidAttr = formContext.getAttribute('new_autoid');

            contactAttr.addOnChange(autoAndContactOnChange);
            autoidAttr.addOnChange(autoAndContactOnChange);
        }
    };
})();

// Task 3
// На объекте Договор, после выбора кредитной программы, становятся доступными для 
// редактирования поля, связанные с расчетом кредита. 

Navicon.nav_agreement.task3 = (function () {

    var creditidOnChange = function (context) {

        let formContext = context.getFormContext();
        let creditAttr = formContext.getAttribute('new_creditid');
        let creditValue = creditAttr.getValue();

        console.log(creditValue);

        if (creditValue) {
            disableCreditControls(context, false);
        } else {
            disableCreditControls(context, true);
        }
    };

    var disableCreditControls = function (context, bool) {

        let formContext = context.getFormContext();

        creditAttributeNames.forEach(controlName => {
            var control = formContext.getControl(controlName);

            if (control) {
                control.setDisabled(bool);
            }
        });
    }

    return {
        onLoad: function (context) {

            let formContext = context.getFormContext();
            let creditAttr = formContext.getAttribute('new_creditid');

            creditAttr.addOnChange(creditidOnChange);
        }
    };
})();

// Task 4
// На объекте Договор, поскольку кредитные программы связаны с объектом 
// Автомобиль отношением N:N то в договоре при выборе кредитной программы 
// в списке лукап поля должны быть доступны только программы, связанные  
// с выбранным объектом Автомобиль.

Navicon.nav_agreement.task4 = (function () {

    let filterContacts = function (context) {
        let formContext = context.getFormContext();
        let autoidAttr = formContext.getAttribute('new_autoid');
        let autoidValue = autoidAttr.getValue();

        if (autoidValue == null || autoidValue.length < 1 || autoidValue[0] == null) return;

        var relationshipPromise = Xrm.WebApi.retrieveMultipleRecords('new_new_credit_new_auto', '?$filter=new_autoid eq ' +
            autoidValue[0].id);

        let filter = "<filter type='and'><condition attribute='new_creditid' operator='in'>";
        let control = formContext.getControl('new_creditid');

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
                console.error(error.message);
            }
        );
    };

    return {
        onLoad: function (context) {

            filterContacts(context);
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

    var dateStartOnChange = function (context) {
        let datesIsNotValid = !isDateEndOneYearLaterThanDateStart(context);

        if (datesIsNotValid) {
            showNotValidDatesNotification(3);
        }
    };

    var entityOnSave = function (context) {
        var datesIsNotValid = !isDateEndOneYearLaterThanDateStart(context);

        if (datesIsNotValid) {
            context.getEventArgs().preventDefault();
            showNotValidDatesNotification(2);
        }
    };

    var isDateEndOneYearLaterThanDateStart = function (context) {
        let formContext = context.getFormContext();
        let dateStartAttr = formContext.getAttribute('new_datestart');
        let dateEndAttr = formContext.getAttribute('new_dateend');
        let dateStartValue = dateStartAttr.getValue();
        let dateEndValue = dateEndAttr.getValue();

        let dateDiff = dateEndValue.getFullYear() - dateStartValue.getFullYear();
        return dateDiff >= 1;
    };

    var showNotValidDatesNotification = function (level) {
        let notification =
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
                console.log(error.message);
            }
        );
    };

    return {
        onLoad: function (context) {
            let formContext = context.getFormContext();
            let dateStartAttr = formContext.getAttribute('new_datestart');
            let dateEndAttr = formContext.getAttribute('new_dateend');

            dateStartAttr.addOnChange(dateStartOnChange);
            dateEndAttr.addOnChange(dateStartOnChange);
            formContext.data.entity.addOnSave(entityOnSave)
        }
    };
})();

// Task 6
// На объекте Договор, реализовать функцию для поля номер договора - 
// по завершении ввода, оставлять только цифры и тире.

Navicon.nav_agreement.task6 = (function () {
    
    var numberOnChange = function (context) {
        let formContext = context.getFormContext();
        let numberAttr = formContext.getAttribute('new_number');
        let numberValue = numberAttr.getValue();

        if (numberValue == null) return;
        let resultNumber = numberValue.replace(/[^\d\-]/g, '');
        numberAttr.setValue(resultNumber);
    };

    return {
        onLoad: function (context) {
            let formContext = context.getFormContext();
            let numberAttr = formContext.getAttribute('new_number');

            numberAttr.addOnChange(numberOnChange);
        }
    };
})();