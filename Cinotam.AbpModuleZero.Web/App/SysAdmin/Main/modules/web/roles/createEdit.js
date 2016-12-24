(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.roles.createEdit', CreateEditController);

    CreateEditController.$inject = ['$uibModalInstance', 'abp.services.app.role', 'items'];
    function CreateEditController($uibModalInstance, _rolesSerivce, items) {
        var vm = this;

        vm.cancel = function () {
            $uibModalInstance.close();
        }
        vm.role = {};
        vm.editPermissionsAllowed = false;
        ////////////////

        function activate() {
            vm.editPermissionsAllowed = abp.auth.isGranted('Pages.SysAdminPermissions');
            _rolesSerivce.getRoleForEdit(items.id).then(function (response) {
                vm.role = response.data;
                for (var i = 0; i < response.data.assignedPermissions.length; i++) {
                    var permission = response.data.assignedPermissions[i];
                    addTreeElement(permission);
                };
                reloadTree();
            });
        }
        vm.treeConfig = {

            "checkbox": {
                keep_selected_style: false,
                three_state: false,
                cascade: ''
            },
            'plugins': ["wholerow", "html_data", "checkbox", "ui"],
            'core': {

                "multiple": true,
                'themes': {
                    'name': 'proton',
                    'responsive': true
                }
            }
        };
        vm.submit = function () {
            vm.role.assignedPermissions = [];
            var selected = vm.tree.jstree('get_selected');
            selected.forEach(function (v) {
                vm.role.assignedPermissions.push({
                    Name: v,
                    Granted: true
                });
            });
            _rolesSerivce.createEditRole(vm.role).then(function () {
                $uibModalInstance.close('roleEdited');
                
            });
        }
        vm.treeData = [];
        vm.treeEventHandlers = {
            "changed.jstree": function (e, data) {
                if (!data.node) {
                    return;
                }
                var childrenNodes;
                if (data.node.state.selected) {
                    selectNodeAndAllParents(vm.tree.jstree('get_parent', data.node));
                    childrenNodes = $.makeArray(vm.tree.jstree('get_children_dom', data.node));
                    vm.tree.jstree('select_node', childrenNodes);

                } else {
                    childrenNodes = $.makeArray(vm.tree.jstree('get_children_dom', data.node));
                    vm.tree.jstree('deselect_node', childrenNodes);
                }
            }
        }
        function selectNodeAndAllParents(node) {
            vm.tree.jstree('select_node', node, true);
            var parent = vm.tree.jstree('get_parent', node);
            if (parent) {
                selectNodeAndAllParents(parent);
            }
        };
        vm.tree = {}
        function reloadTree() {
            vm.treeConfig.version++;
        }
        activate();
        function addTreeElement(permission) {
            var model = new treeObj(permission.name, permission.parentPermission, permission.displayName, permission.granted);
            vm.treeData.push(model);
            if (permission.childPermissions.length > 0) {
                for (var i = 0; i < permission.childPermissions.length; i++) {
                    addTreeElement(permission.childPermissions[i]);
                }
            }
        }
        function treeObj(id, parent, text, checked) {
            if (!parent) {
                parent = "#";
            }
            this.id = id;
            this.parent = parent;
            this.text = text;
            this.state = {
                opened: true,
                selected: checked
            }
            return this;
        }

    }
})();