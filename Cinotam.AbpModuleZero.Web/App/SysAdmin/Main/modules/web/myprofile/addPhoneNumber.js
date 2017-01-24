(function() {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.myprofile.changePhone', ChangePhoneController);

    ChangePhoneController.$inject = ['$uibModalInstance', 'appSession', 'abp.services.app.user'];
    function ChangePhoneController($uibModalInstance, appSession, _userService) {
        var vm = this;
        vm.selectedCountryCode = {
            name: 'Mexico (+52)',
            countryCode: 'MX',
            code: '52'
        }
        vm.supportedCountryCodes = [
            {
                name: 'Mexico (+52)',
                countryCode: 'MX',
                code: '52'
            }
        ];
        vm.currentFunc = 'addphone';
        vm.callFunc = function(current) {
            if (vm.currentFunc === 'addphone') {
                vm.submit();
            }
            else {
                vm.confirmCode();
            }
        }
        vm.postData = {};
        vm.submit = function() {
            vm.postData = {
                userId: appSession.user.id,
                phoneNumber: vm.phoneInput,
                countryPhoneCode: vm.selectedCountryCode.code,
                countryCode: vm.selectedCountryCode.countryCode
            };
            abp.ui.setBusy();
            _userService.addPhoneNumber(vm.postData).then(function(response) {

                if (response.data.resultType === 0) {
                    $uibModalInstance.close('samenumber');
                } else {
                    //
                    vm.showCaptureWindow = false;
                    vm.showConfirmWindow = true;
                    vm.buttonText = App.localize('Confirm');
                    vm.currentFunc = 'confirm';
                }
                abp.ui.clearBusy();
            }).catch(function() {
                abp.ui.clearBusy();
            });
        }

        vm.buttonText = App.localize('Save');
        vm.cancel = function() {
            $uibModalInstance.close();
        }
        vm.token = '';
        vm.confirmCode = function() {
            abp.ui.setBusy();
            vm.postData.token = vm.token;
            _userService.confirmPhone(vm.postData).then(function(response) {
                abp.ui.clearBusy();
                $uibModalInstance.close('confirmed');

            }).catch(function() {
                abp.ui.clearBusy();
            });
        }
        vm.showConfirmWindow = false;
        vm.showCaptureWindow = true;
        vm.phoneInput = "";
        activate();

        ////////////////

        function activate() { }
    }
})();