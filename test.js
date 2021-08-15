var Navicon = Navicon || {};

Navicon.nav_building = (function() {

    var floorOnChange = function(context) {
        
    }

    return {
        onLoad : function (context) {
            let formContext = context.getFormContext();
            let floorAttr = formContext.getAttribute('attributeName');        
            floorAttr.addOnChange(floorOnChange);
        }
    }
})()
