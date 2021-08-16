var Navicon = Navicon || {};
const creatingFormType = 1;
const enabledControlsForCreating = ['new_number', 'new_date', 'new_contact', 'new_autoid'];

// Task 1
// При создании объекта Договор, сразу после открытия карточки доступны для редактирования 
// поля: номер, дата договора, контакт и модель. Остальные поля - скрыты. 
// Вкладка с данными по кредиту скрыта.  


Navicon.nav_agreement.tas1 = (function () {

    var disableCreationFormAttributes = function (context) {
        let formContext = context.getFormContext();
        var controls = formContext.getControl();

        console.log('Controls:');

        controls.forEach(control => {
            let controlName = control.getName();
            if (!enabledControlsForCreating.includes(controlName)) {
                control.setDisabled(true);
            }
        });

        var tabObj = formContext.ui.tabs.get('Credit');
        console.log(tabObj);
        tabObj.setVisible(false);
    };

    return {
        onLoad: function (context) {

            let formContext = context.getFormContext();
            let formType = formContext.ui.getFormType();

            if (formType == creatingFormType) {
                disableCreationFormAttributes(context);
            }

            let contactAttr = formContext.getAttribute('new_contact');
            let autoidAttr = formContext.getAttribute('new_autoid');

            contactAttr.addOnChange(floorOnChange);
            autoidAttr.addOnChange(floorOnChange);
        }
    };
})();