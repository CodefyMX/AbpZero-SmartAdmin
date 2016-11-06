/*
 * SMART CHAT ENGINE (EXTENTION)
 * Copyright (c) 2013 Wen Pu
 * Modified by MyOrange
 * All modifications made are hereby copyright (c) 2014-2015 MyOrange
 */

// clears the variable if left blank
// Need this to make IE happy
// see http://soledadpenades.com/2007/05/17/arrayindexof-in-internet-explorer/
/*if (!Array.indexOf) {
	Array.prototype.indexOf = function (obj) {
		for (var i = 0; i < this.length; i++) {
			if (this[i] == obj) {
				return i;
			}
		}
		return -1;
	}
}*/

var chatboxManager = function () {
    var _chatService = abp.services.app.chat;
    var init = function (options) {
        $.extend(chatbox_config, options);

    };


    var delBox = function (id) {
        // TODO
    };

    var getNextOffset = function () {
        return (chatbox_config.width + chatbox_config.gap) * showList.length;
    };

    var boxClosedCallback = function (id) {
        // close button in the titlebar is clicked

        console.log("ClosedId:",id);

        removeFromKeepAliveList(id);

        var idx = showList.indexOf(id);
        if (idx != -1) {
            showList.splice(idx, 1);
            var diff = chatbox_config.width + chatbox_config.gap;
            for (var i = idx; i < showList.length; i++) {
                var offset = $("#" + showList[i]).chatbox("option", "offset");
                $("#" + showList[i]).chatbox("option", "offset", offset - diff);
            }
        } else {
            alert("NOTE: Id missing from array: " + id);
        }
    };
    var sendToServer = function (user, message) {
        console.log("Current message info", user);

        var data = {
            From: user.currentUserId,
            To: user.toUserId,
            Message: message

        }


        _chatService.sendMessage(data).done(function () {
            console.log("Send to server");
        });


    }
    // not used in demo
    var dispatch = function (id, user, msg) {
        //$("#log").append("<i>" + moment().calendar() + "</i> you said to <b>" + user.first_name + " " + user.last_name + ":</b> " + msg + "<br/>");
        if ($('#chatlog').doesExist()) {
            $("#chatlog").append("You said to <b>" + user.first_name + " " + user.last_name + ":</b> " + msg + "<br/>").effect("highlight", {}, 500);;
        }
        sendToServer(user, msg);

        $("#" + id).chatbox("option", "boxManager").addMsg(LSys("Me"), msg);
    }
    var pushMessage = function (id, user, msg) {
        $("#" + id).chatbox("option", "boxManager").addMsg(user.first_name + " " + user.last_name, msg);
    }
    // caller should guarantee the uniqueness of id
    var addBox = function (idConversation, user, success) {

        if (!success) success = function () { };

        var data = {
            From: user.currentUserId,
            To: user.toUserId
        }
        _chatService.createConversation(data).done(function (id) {
            if (id != 0) {
                var idx1 = showList.indexOf(id);
                var idx2 = boxList.indexOf(id);
                if (idx1 != -1) {
                    // found one in show box, do nothing
                } else if (idx2 != -1) {
                    // exists, but hidden
                    // show it and put it back to showList
                    $("#" + id).chatbox("option", "offset", getNextOffset());
                    var manager = $("#" + id).chatbox("option", "boxManager");
                    manager.toggleBox();
                    showList.push(id);
                } else {
                    var el = document.createElement('div');
                    el.setAttribute('id', id);
                    $(el)
						.chatbox({
						    id: id,
						    user: user,
						    title: '<i title="' + user.status + '"></i>' + user.first_name + " " + user.last_name,
						    hidden: false,
						    offset: getNextOffset(),
						    width: chatbox_config.width,
						    status: user.status,
						    alertmsg: user.alertmsg,
						    alertshow: user.alertshow,
						    messageSent: dispatch,
						    boxClosed: boxClosedCallback
						});
                    boxList.push(id);
                    showList.push(id);
                    nameList.push(user.first_name);

                }
            } else {
                console.log("No conversation available");
            }
            success();
            keepAlive(id, user);
        });




    };

    var messageSentCallback = function (id, user, msg) {
        var idx = boxList.indexOf(id);
        chatbox_config.messageSent(nameList[idx], msg);
    };

    var keepAliveElements = [];
    //Functions to show the active message boxes when page changes or reloads
    function keepAlive(conversationId, user) {

        if (keepAliveElements.length <= 0) {
            var cValue = Cookies.get("keepAliveChats");
            if (cValue) {
                keepAliveElements = JSON.parse(cValue);

            }
        }
        if (!cookieExists(conversationId)) {

            keepAliveElements.push({
                conversationId: conversationId,
                user: user
            });

            Cookies.set("keepAliveChats", keepAliveElements);
        }
    }
    function cookieExists(conversationId) {
        for (var i = 0; i < keepAliveElements.length; i++) {
            if (keepAliveElements[i] === conversationId) {
                return true;
            }
        }
        return false;
    }
    function removeFromKeepAliveList(conversationId) {

        var list = getKeepAliveList();

        var editedList = removeElementFromList(list, conversationId);

        Cookies.set("keepAliveChats", editedList);

    }
    function removeElementFromList(list, conversationId) {

        return list.filter(function (element) {

            return element.conversationId !== conversationId;

        });
    }
    function getKeepAliveList() {
        if (keepAliveElements.length <= 0) {
            var cValue = Cookies.get("keepAliveChats");
            if (cValue) {
                keepAliveElements = JSON.parse(cValue);

            }
        }
        return keepAliveElements;
    }
    function loadChatWindows() {
        console.log("Loading");
        if (keepAliveElements.length <= 0) {
            var cValue = Cookies.get("keepAliveChats");

            if (cValue) {
                keepAliveElements = JSON.parse(cValue);

            }
        }
        for (var i = 0; i < keepAliveElements.length; i++) {
            var conversationId = keepAliveElements[i].conversationId;
            var user = keepAliveElements[i].user;
            addBox(conversationId, user);
        }

    }
    loadChatWindows();
    return {
        init: init,
        addBox: addBox,
        delBox: delBox,
        dispatch: dispatch,
        pushMessage: pushMessage,
        loadChatWindows: loadChatWindows
    };
}();


$('a[data-chat-id]:not(.offline)').click(function (event, ui) {

    var $this = $(this),
		temp_chat_id = $this.attr("data-chat-id"),
		fname = $this.attr("data-chat-fname"),
		lname = $this.attr("data-chat-lname"),
		status = $this.attr("data-chat-status") || "online",
		alertmsg = $this.attr("data-chat-alertmsg"),
		alertshow = $this.attr("data-chat-alertshow") || false,
		currentUserId = abp.session.userId,
		toUserId = $this.attr("data-to-id");

    if (toUserId == currentUserId) return false;

    chatboxManager.addBox(temp_chat_id, {
        // dest:"dest" + counter, 
        // not used in demo
        title: "username" + temp_chat_id,
        first_name: fname,
        last_name: lname,
        status: status,
        alertmsg: alertmsg,
        alertshow: alertshow,
        currentUserId: currentUserId,
        chatId: temp_chat_id,
        toUserId: toUserId
        //you can add your own options too
    });

    event.preventDefault();

});