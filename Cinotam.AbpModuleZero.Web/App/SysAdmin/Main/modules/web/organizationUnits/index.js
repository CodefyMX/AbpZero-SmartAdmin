(function() {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.organizationUnits.index', OrganizationUnitsController);

    OrganizationUnitsController.$inject = ['abp.services.app.organizationUnits'];
    function OrganizationUnitsController(_orgUnits) {
        var vm = this;
        vm.treeData = [
   
        ];
        vm.treeConfig = {
            core : {
                themes: {
                    name: 'proton',
                    responsive: true
                }
            },
            version : 1
        }
        activate();

        ////////////////

        function activate() {

            _orgUnits.getOrganizationUnitsConfigModel().then(function (response) {
                var treeModel = response.data;
                for (var i = 0; i < treeModel.organizationUnits.length; i++) {
                    buildTreeData(treeModel.organizationUnits[i]);
                }
                reloadTree();
            });

        }

        function reloadTree(){
            vm.treeConfig.version ++;
        }
        function buildTreeData(treeElm) {
            var model = new treeObj(treeElm.id,treeElm.parentId,treeElm.displayName);
            console.log(model);
            vm.treeData.push(model);
            if(treeElm.childrenDto.length>0){
                for (var i = 0; i < treeElm.childrenDto.length; i++) {
                    buildTreeData(treeElm.childrenDto[i]);
                }
            }
        }

        var treeObj = function (id, parent, text) {
            if (parent) {
                parent = "unique" + parent;
            } else {
                parent = "#";
            }
            this.id = "unique"+id;
            this.parent = parent;
            this.text = text;
            return this;
        }

    }

    
})();