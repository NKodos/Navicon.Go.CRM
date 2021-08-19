var Navicon = Navicon || {};
Navicon.nav_agreement = Navicon.nav_agreement || {};

// Task 1
// Объект Договор: после выбора значения в поле кредитная программа, 
// проверять ее срок действия относительно даты договора. Если срок истек, 
// система должна показывать пользователю сообщение и блокировать 
// сохранение формы.
// 1. Вытащить дату окончания кр. программы
// 2. Проверка: датой договора <= даты окончания кр. пр.
// 3. Показ алерта

Navicon.nav_agreement.task1 = (function () {

    var disableCreationFormAttributes = function (context) {
        let formContext = context.getFormContext();
        let controls = formContext.getControl();

        if (controls == null) return;

        controls.forEach(control => {
            let controlName = control.getName();
            if (!enabledControlsForCreating.includes(controlName)) {
                control.setDisabled(true);
            }
        });

        let creditTab = formContext.ui.tabs.get('Credit');
        if (creditTab != null) {
            creditTab.setVisible(false);
        }
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