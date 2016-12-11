(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.users.changePermissions', ChangePermissionsController);

    ChangePermissionsController.$inject = ["$uibModalInstance", "items", "abp.services.app.user"];
    function ChangePermissionsController($uibModalInstance, items, _userService) {
        var vm = this;
        vm.cancel = function () {
            $uibModalInstance.close();
        }

        var data = {
            AssignedPermissions: [],
            UserId: items.userId
        }


        vm.resetPermissions = function(){
            _userService.resetAllPermissions(items.userId).then(function(){
                $uibModalInstance.close("permissionsset")
            });
        }

        vm.submit = function () {

            var selected = vm.tree.jstree('get_json');
            selected.forEach(function (v) {
                var granted = v.state.selected;
                data.AssignedPermissions.push({
                    Name: v.id,
                    Granted: granted
                });
                getPermissionsForChildren(v.children);
            });
            _userService.setUserSpecialPermissions(data).then(function () {
                $uibModalInstance.close("permissionsset")
            });
        }
        function getPermissionsForChildren(children) {
            children.forEach(function (v) {
                var granted = v.state.selected;
                data.AssignedPermissions.push({
                    Name: v.id,
                    Granted: granted
                });
                getPermissionsForChildren(v.children);
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

        ////////////////

        function activate() {
            _userService.getUserSpecialPermissions(items.userId).then(function (response) {
                console.log(response.data);
                for (var i = 0; i < response.data.assignedPermissions.length; i++) {
                    var permission = response.data.assignedPermissions[i];
                    addTreeElement(permission);
                };
                reloadTree();
            });
        }

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