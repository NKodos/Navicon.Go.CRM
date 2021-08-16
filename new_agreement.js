var Navicon = Navicon || {};
Navicon.nav_agreement = Navicon.nav_agreement || {};
const creatingFormType = 1;
const enabledControlsForCreating = ['new_number', 'new_date', 'new_contact', 'new_autoid'];

// Task 1
// При создании объекта Договор, сразу после открытия карточки доступны для редактирования 
// поля: номер, дата договора, контакт и модель. Остальные поля - скрыты. 
// Вкладка с данными по кредиту скрыта.

Navicon.nav_agreement.task1 = (function () {

    var disableCreationFormAttributes = function (context) {
        let formContext = context.getFormContext();
        let controls = formContext.getControl();

        console.log('Controls:');

        controls.forEach(control => {
            let controlName = control.getName();
            if (!enabledControlsForCreating.includes(controlName)) {
                control.setDisabled(true);
            }
        });

        let creditTab = formContext.ui.tabs.get('Credit');
        console.log(creditTab);
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

        console.log(`contact=${contactAttrValue}, auto=${autoidAttrValue}`);

        let creditTab = formContext.ui.tabs.get('Credit');

        if (contactAttrValue && autoidAttrValue) {
            creditTab.setVisible(true);
        } else {
            creditTab.setVisible(false);
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