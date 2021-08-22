var Navicon = Navicon || {};

// Task
// 1. +++Создать новый js ресурс (new_relationship_credit_model_iframe.js)
// 2. +++Создать html страницу (new_relationship_credit_model_iframe.html), 
// подключить скрипты, добавить на форму.
// 3. +++Получить данные и сформировать массив данных в скрипте.
// 4. Создать таблицу в HTML странице и оформить ее по стилю с CRM
// 5. Отобразить данные в таблице.
// 6. Реализовать события нажатия на сущности модели и кредитной программы. 
// Событие должно открывать отдельные окна сущностей.

Navicon.new_relationship_credit_model_iframe = (function () {

    return {
        onLoad: function (context) {
            const formContext = context.getFormContext();
            const brandId = formContext.data.entity.getId();
            console.log(brandId);

            if (!brandId) {
                console.error('id сущности не был найден')
                return;
            }

            const promise = Xrm.WebApi.retrieveMultipleRecords(
                'new_agreement',
                '?$select=new_name,new_creditperiod' +
                '&$expand=new_autoid($select=new_modelid,new_name;$expand=new_modelid($select=new_name),' +
                'new_brendId($select=new_brandid)),' +
                'new_creditid($select=new_name)'
            );

            promise.then(
                function (resultArray) {
                    const resultArrayFiltered = resultArray.entities.filter(
                        (entity, index) => entity.new_autoid.new_brendId.new_brandid.toLowerCase() === '76B4E016-E8FC-EB11-94EF-002248995083'.toLowerCase());


                    const gridObjectArray = resultArrayFiltered.map(
                        (entity, index) => {
                            return {
                                creditId: entity.new_creditid?.new_creditid,
                                creditName: entity.new_creditid?.new_name,
                                modelId: entity.new_autoid?.new_modelid?.new_modelid,
                                modelName: entity.new_autoid?.new_modelid?.new_name,
                                creditPeriod: entity.new_creditperiod
                            };
                        }
                    );

                    const resourceControl = formContext.getControl('WebResource_new_relationship_credit_model_iframe').getContentWindow();
                    resourceControl.then(
                        function (contentWindow) {
                            contentWindow.fillGrid(gridObjectArray);
                        },
                        function (error) {
                            console.error(error.message);
                        }
                    );
                },
                function (error) {
                    console.error(error.message);
                }
            );
        }
    };
})();

// let objArray = [
//     { name: 'name1', age: 20 },
//     { name: 'name2', age: 21 },
//     { name: 'name2', age: 21 },
//     { name: 'name3', age: 22 },
// ];
// console.log(objArray);

// objArray = objArray.filter((item, index) => {
//     return objArray.indexOf(item) === index;
// });
// console.log(objArray);