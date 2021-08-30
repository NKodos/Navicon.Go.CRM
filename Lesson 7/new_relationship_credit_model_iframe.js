var Navicon = Navicon || {};

// Объект, для iframe страницы моделей, который загружает
// информацию в таблицу, отображающую автомобили и кредные программы,
// в которых учавствуют соответствующие автомобили
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
                        function (entity, index) {
                            entity.new_autoid.new_brendId.new_brandid.toLowerCase() === '76B4E016-E8FC-EB11-94EF-002248995083'.toLowerCase();
                        }
                    );


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