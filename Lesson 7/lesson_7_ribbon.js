var Navicon = Navicon || {};
Navicon.lesson_7 = Navicon.lesson_7 || {};


// Task 4
// На карточку объекта Договор поместить кнопку «Пересчитать кредит». При нажатии на кнопку 
// - Пересчитывать поле сумма кредита.
// Сумма кредита = [Договор].[Сумма]–[Договор].[Первоначальный взнос]
// - Пересчитать поле полная стоимость кредита
// полная стоимость кредита = 
// ([Кредитная Программа].[Ставка] / 100 * [Договор].[Срок кредита] * 
// [Договор].[Сумма кредита]) + [Договор].[Сумма кредита]

Navicon.lesson_7.task4 = (function () {
    const alertDelay = 5000;

    const calculateCreditAmount = function () {
        let creditAmountAttr = Xrm.Page.getAttribute('new_creditamount');
        let summaAttr = Xrm.Page.getAttribute('new_summa');
        let initialFeeAttr = Xrm.Page.getAttribute('new_initialfee');

        if (creditAmountAttr == null || summaAttr == null) return;
        let creditAmount = summaAttr.getValue() - initialFeeAttr.getValue();
        creditAmountAttr.setValue(creditAmount);
    };

    const calculateFullCreditAmount = function () {
        let creditIdAttr = Xrm.Page.getAttribute('new_creditid');
        let fullCreditAmountAttr = Xrm.Page.getAttribute('new_fullcreditamount');
        let creditPeriodAttr = Xrm.Page.getAttribute('new_creditperiod');
        let creditAmountAttr = Xrm.Page.getAttribute('new_creditamount');

        console.log(fullCreditAmountAttr, creditPeriodAttr);
        if (fullCreditAmountAttr == null || creditPeriodAttr == null) return;

        let creditPeriodValue = creditPeriodAttr.getValue();
        let creditAmountValue = creditAmountAttr.getValue();

        if (creditPeriodValue == null ||
            creditPeriodValue == 0 ||
            creditAmountValue == null ||
            creditAmountValue == 0) {
            showNotification('Сумма кредита и срок кредита не должны равняться нулю', 2);
            return;
        }

        let creditIdValue = creditIdAttr.getValue();
        if (creditIdValue == null || creditIdValue.length < 1) return;

        if (creditIdValue == null) {
            showNotification('Кредитная программа не выбрана', 3);
            return;
        }

        let creditPromise = Xrm.WebApi.retrieveRecord('new_credit', creditIdValue[0].id,
            '?$select=new_persent');

        creditPromise.then(
            function (creditEntity) {
                let result = (creditEntity.new_persent / 100 * 
                    creditPeriodValue * creditAmountValue) + creditAmountValue;

                fullCreditAmountAttr.setValue(result);
            },
            function (error) {
                console.log(error.message);
            }
        );
    };

    var showNotification = function (message, level) {
        let notification =
        {
            type: 2,
            level: level,
            showCloseButton: true,
            message: message
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
        calculateCreditSumm: function () {
            let creditIdAttr = Xrm.Page.getAttribute('new_creditid');
            if (creditIdAttr == null || creditIdAttr.getValue() == null) return;

            calculateCreditAmount();
            calculateFullCreditAmount();
        }
    };
})();