(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.organizationUnits.index', OrganizationUnitsController);

    OrganizationUnitsController.$inject = ['abp.services.app.organizationUnits', '$uibModal'];
    function OrganizationUnitsController(_orgUnits, modal) {
        var vm = this;
        vm.treeData = [

        ];

        //Tree instance
        vm.tree = {};

        var contextMenu = function (node) {
            var items = {
                addUnit: {
                    label: App.localize('AddOrganizationUnit'),
                    action: function () {
                        vm.openCreateModal(null, node.id);
                    }
                },
                editUnit: {
                    label: App.localize('EditOrganizationUnit'),
                    action: function () {
                        vm.openCreateModal(node.id, null);
                    }
                },
                deleteUnit: {
                    label: App.localize('DeleteOrganizationUnit'),
                    action: function () {
                        abp.message.confirm(App.localize("TheUnitWillBeDeleted"), App.localize("ConfirmQuestion"), function (response) {
                            if (response) {
                                abp.ui.setBusy();
                                _orgUnits.removeOrganizationUnit(node.id).then(function () {
                                    abp.ui.clearBusy();

                                    vm.removeFromTree(node);
                                    reloadTree();
                                });
                            }
                        });
                    }
                }
            }

            return items;
        }

        vm.loadUsersView = function(){
            
        }

        vm.removeFromTree = function (node) {
            for (var i = 0 ; i < vm.treeData.length; i++) {
                if (vm.treeData[i].id == node.id) {
                    vm.treeData.splice(i, 1);
                }
            }
        }
        //var oldParent;
        //var oldPosition;
        //function handleStart(e, data) {
        //    //I need the current element position.... mmmm
        //    /*
        //        I've got to use my head. And think.
        //        Hmm... Mmm...  (Sleep gesture)
        //     */
        //    oldPosition = vm.tree.jstree(true).get_node(data.data.nodes[0]);
        //    console.log(oldPosition);
        //    oldParent = vm.tree.jstree(true).get_node(data.data.nodes[0]).parent;
        //}
        var handleStop = function(e, data) {
            var node = data.data.origin.get_node(data.data.nodes[0]);
            if (node.type === "root") return false;
            var elementMoved = data.data.nodes[0];
            var newParent = node.parent;
            var request = {
                Id: elementMoved,
                ParentId: newParent
            }
            e.preventDefault();
            _orgUnits.moveOrgUnit(request)
                .then(function() {});
        }
        //This is excecuting twice..... mmmmmmm!!
        vm.treeEventHandlers = {
            'dnd_stop.vakata': handleStop
        }
        vm.treeConfig = {
            contextmenu: {
                items: contextMenu
            },
            core: {
                themes: {
                    name: 'proton',
                    responsive: true
                },
                check_callback: true,
            },
            plugins: ["wholerow", "html_data", "ui", "contextmenu", "dnd"],
            version: 1
        }
        activate();
        vm.openCreateModal = function (id, parentId) {
            var modalInstance = modal.open({
                templateUrl: '/App/SysAdmin/Main/modules/web/organizationUnits/create.cshtml',
                controller: 'app.views.organizationUnits.create as vm',
                resolve: {
                    items: function () {

                        var paramsObj = {};

                        if (id) {
                            paramsObj.id = id;
                        }

                        if (parentId) {
                            paramsObj.parentId = parentId;
                        }

                        return paramsObj;
                    }
                }
            });
            modalInstance.result.then(function (result) {
                if (result == 'created') {
                    activate();
                }
            });
        }
        ////////////////

        function activate() {
            vm.treeData = [];
            _orgUnits.getOrganizationUnitsConfigModel().then(function (response) {
                var treeModel = response.data;
                for (var i = 0; i < treeModel.organizationUnits.length; i++) {
                    buildTreeData(treeModel.organizationUnits[i]);
                }
                reloadTree();
            });

        }

        function reloadTree() {
            vm.treeConfig.version++;
        }
        function buildTreeData(treeElm) {
            var model = new treeObj(treeElm.id, treeElm.parentId, treeElm.displayName);
            vm.treeData.push(model);
            if (treeElm.childrenDto.length > 0) {
                for (var i = 0; i < treeElm.childrenDto.length; i++) {
                    buildTreeData(treeElm.childrenDto[i]);
                }
            }
        }

        var treeObj = function (id, parent, text) {
            if (!parent) {
                parent = "#";
            }
            this.id = +id;
            this.parent = parent;
            this.text = text;
            this.state = { opened: true }
            return this;
        }

    }


})();