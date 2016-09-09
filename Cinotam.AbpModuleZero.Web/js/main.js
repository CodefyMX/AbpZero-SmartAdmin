(function ($) {



    //serializeFormToObject plugin for jQuery
    $.fn.serializeFormToObject = function () {
        //serialize to array
        var data = $(this).serializeArray();

        //add also disabled items
        $(':disabled[name]', this).each(function () {
            data.push({ name: this.name, value: $(this).val() });
        });

        //map to object
        var obj = {};
        data.map(function (x) { obj[x.name] = x.value; });

        return obj;
    };

    //Configure blockUI
    if ($.blockUI) {
        $.blockUI.defaults.baseZ = 2000;
    }
})(jQuery);


var notificationService = (function () {
    //Notification handler
    abp.event.on('abp.notifications.received', function (userNotification) {
        abp.notifications.showUiNotifyForUserNotification(userNotification);
    });
    function initView($element) {
        if (!$element) {
            $element = $("#userNotifications");
        }
        $element.text("");
        abp.services.app.user.getMyNotifications(2, 10).done(function (response) {
            response.notifications.forEach(function (userNotification) {
                printNotificationInList(userNotification);
            });
        });
    }
    function printNotificationInList(userNotification, $element) {
        if (!$element) {
            $element = $("#userNotifications");
            console.info("Element not defined, defining default");
        } else {
            console.info("Element defined");
        }
        if (userNotification.notification.data.type === 'Abp.Notifications.LocalizableMessageNotificationData') {
            var localizedText = abp.localization.localize(
                userNotification.notification.data.message.name,
                userNotification.notification.data.message.sourceName
            );

            $.each(userNotification.notification.data.properties, function (key, value) {
                localizedText = localizedText.replace('{' + key + '}', value);
            });

            var stateClass = getStateClass(userNotification);
            var html;

            var badgeColors = {
                blue: "badge padding-5 no-border-radius bg-color-blue pull-left margin-right-5",
                green: "badge padding-5 no-border-radius bg-color-green pull-left margin-right-5",
                yellow: "badge padding-5 no-border-radius bg-color-yellow pull-left margin-right-5"

            }
            var icons = {
                roles: "fa fa-lock fa-fw fa-2x",
                users: "fa fa-users fa-fw fa-2x",
                user: "fa fa-user fa-fw fa-2x",
                languages: "fa fa-flag fa-fw fa-2x"
            }


            switch (userNotification.notification.notificationName) {

                case "RoleAssignedToUser":
                    //badge padding-5 no-border-radius bg-color-blue pull-left margin-right-5
                    //fa fa-lock fa-fw fa-2x
                    html = getHtmlForNotification(userNotification, badgeColors.blue, icons.roles, stateClass, "/SysAdmin/Users/MyProfile", "");
                    setHtmlNotification($element, html, localizedText);
                    break;
                case "RoleAssigned":
                    html = getHtmlForNotification(userNotification, badgeColors.blue, icons.roles, stateClass, "/SysAdmin/Users/UsersAndRolesList/?userId=", userNotification.notification.data.properties.userId);
                    setHtmlNotification($element, html, localizedText);
                    break;
                case "RoleCreated":
                    html = getHtmlForNotification(userNotification, badgeColors.green, icons.roles, stateClass, "/SysAdmin/Users/UsersAndRolesList/", "");
                    setHtmlNotification($element, html, localizedText);
                    break;
                case "RoleDeleted":
                    html = getHtmlForNotification(userNotification, badgeColors.yellow, icons.roles, stateClass, "/SysAdmin/Users/UsersAndRolesList/", "");
                    setHtmlNotification($element, html, localizedText);
                    break;
                case "LanguageCreated":
                    html = getHtmlForNotification(userNotification, badgeColors.blue, icons.languages, stateClass, "/SysAdmin/Languages/LanguagesList/", "");
                    setHtmlNotification($element, html, localizedText);
                    break;
                case "LanguageDeleted":
                    html = getHtmlForNotification(userNotification, badgeColors.yellow, icons.languages, stateClass, "/SysAdmin/Languages/LanguagesList/", "");
                    setHtmlNotification($element, html, localizedText);
                    break;
                case "UserCreated":
                    html = getHtmlForNotification(userNotification, badgeColors.green, icons.user, stateClass, "/SysAdmin/Users/UsersAndRolesList/", "");
                    setHtmlNotification($element, html, localizedText);

                    break;
                case "UserDeleted":
                    html = getHtmlForNotification(userNotification, badgeColors.yellow, icons.user, stateClass, "/SysAdmin/Users/UsersAndRolesList/", "");
                    setHtmlNotification($element, html, localizedText);

                    break;
                default:
                    $element.append(abp.utils.formatString('<li class=' + userNotification.id + '>' +
               '<span class="padding-10 ' + stateClass + '">' +
               '<em class="badge padding-5 no-border-radius bg-color-yellow pull-left margin-right-5"> <i class="fa fa-user fa-fw fa-2x"></i></em>' +
               '<span>{0}<br><a data-notification-id=' + userNotification.id + ' href="#" class="js-mark-readed">Mark as readed</a></span>' +
               '</span></li>', localizedText));
                    break;
            }


        } else if (userNotification.notification.data.type === 'Abp.Notifications.MessageNotificationData') {
            alert('New simple notification: ' + userNotification.notification.data.message);
        }
    }
    function getHtmlForNotification(userNotification, badgeClass, icon, stateClass, href, id) {
        //badge padding-5 no-border-radius bg-color-blue pull-left margin-right-5
        //fa fa-lock fa-fw fa-2x

        if (!badgeClass) { badgeClass = "badge padding-5 no-border-radius bg-color-blue pull-left margin-right-5" }
        if (!icon) { icon = "fa fa-user fa-fw fa-2x" }
        return '<li class=' +
            userNotification.id +
            '>' +
            '<span class="padding-10 ' +
            stateClass +
            '">' +
            '<em class="' + badgeClass + '"> <i class="' + icon + '"></i></em>' +
            '<span>{0} <a data-notification-id=' +
            userNotification.id +
            ' data-href="' + href + id + '" class="btn pull-right btn-xs btn-primary margin-top-5 js-call-action">Details</a><br><a href="#" data-notification-id=' +
            userNotification.id +
            ' class="js-mark-readed">Mark as readed</a></span>' +
            '</span></li>';
    }
    function setHtmlNotification($elm, html, localizedText) {
        $elm.append(abp.utils.formatString(html, localizedText));
    }
    function getStateClass(userNotification) {
        if (userNotification.state == 0) {
            return "unread";
        } else {
            return "";
        }
    }
    function startListening() {
        $("body").on("click", ".js-call-action", function () {
            var $elm = $(this);
            callAction($elm);
        });
        $("body").on("click", ".js-mark-readed", function () {
            var $elm = $(this);
            var id = $elm.data("notification-id");
            markAsReaded(id, $elm);
        });
    }
    function callAction($elm) {
        var id = $elm.data("notification-id");
        markAsReaded(id, $elm);
    }
    function markAsReaded(notificationId, $elm) {
        abp.ui.setBusy("#userNotifications", abp.services.app.user.markAsReaded(notificationId).done(function () {
            var selector = "." + notificationId;
            var elements = $ (selector);
            elements.children().removeClass("unread");
            var href = $elm.data("href");
            if (href) {
                window.location.href = href;
            }

        }));
    }

    var functions = {
        initView: initView,
        printNotificationInList: printNotificationInList,
        getHtmlForNotification: getHtmlForNotification,
        setHtmlNotification: setHtmlNotification,
        getStateClass: getStateClass,
        startListening: startListening,
        callAction: callAction,
        markAsReaded: markAsReaded
    };
    return functions;
})();
