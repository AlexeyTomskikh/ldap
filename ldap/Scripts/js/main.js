
$('#calendar').fullCalendar({
    theme: true,
    dayClick: function (date, jsEvent, view) {
        $('#calendar').fullCalendar('changeView', 'agendaDay', date);
    },
    eventLimit: true,
    displayEventTime: true,
    displayEventEnd: true,
    BusinessHours: false,
    nextDayThreshold: '00:00:00',
    selectable: true,
    editable: true,
    customButtons: {
        createEvent: {
            text: 'Добавить мероприятие',
            click: function () {
                $('#dialog').dialog({
                    title: "Новое событие",
                    show: {
                        effect: 'drop',
                        duration: 500
                    },
                    hide: {
                        effect: 'clip',
                        duration: 500
                    }
                });

                $.ajax({
                    url: '/Logic/GetAllUsers/',
                    dataType: 'json',
                    success: function (data) {
                        $('#selectmembers').html(data);
                        for (var i = 0; i < data.length; i++) {
                            $('#selectmembers').append('<option value="' + data[i].id + '">' + data[i].firstName + '</option>');
                        }
                        $('#selectmembers').selectpicker('refresh');
                    }
                });

            }
        }
    },

    header: {
        left: 'prev,next today createEvent',
        center: 'title',
        right: 'month '
    },
    defaultView: 'month',
    height: 800,

    allDaySlot: false, //убираем события длиною в день
    slotEventOverlap: false, // запрещаем пересечение событий

    //загрузчик событий в календарь
    events: function (start, end, timezone, callback) {

        $.ajax({
            url: '/Logic/GetEvents/',
            //url: '/Scheduler/Logic/GetEvents/',
            dataType: 'json',
            data: {
                start: start.unix(),
                end: end.unix()
            },
            success: function (doc) {
                var events = doc;
                callback(events);
            }
        });
    },
    
    timeFormat: 'HH:mm',

    // Клик по событию открывает всплывающее окно
    eventClick: function (calEvent, jsEvent, view) {
        $('#eventinfo #members').empty();  // очищаем список участников
        $('#btnDelete').attr("cust", calEvent.id);
        $('#eventinfo #when').text(calEvent.start.format("D MMM HH:mm") + " - " + calEvent.end.format("HH:mm"));
        $('#eventinfo #description').text(calEvent.descriptionEvent);
        var eventId = calEvent.id;

        $.ajax({
            type: 'POST',
            url: '/Logic/GetMembers?eventId=' + eventId,
            contentType: 'application/json; charset=utf-8',
            processData: false,
            success: function (data) {

                for (var i = 0; i <= data.length; i++) {
                    if (i == data.length - 1) {
                        $('#eventinfo #members').append(data[i].firstName + '.');

                    }
                    if (i != data.length - 1) {
                        $('#eventinfo #members').append(data[i].firstName + ', ');
                    }
                }
            },
            error: function () {
                alert('Ошибка! Проверьте соединение!');
            }
        });


        var roomId = calEvent.room;
        $('#eventinfo #location').text(roomId == 1 ? "Комната 1" : "Комната 2");

        $('#eventinfo').dialog({
            title: calEvent.title,
            show: {
                effect: 'drop',
                duration: 500
            },
            hide: {
                effect: 'clip',
                duration: 500
            }
        });
    }
});


$(document).on("click", ".btn-addevent", function (e) {

    


    var btn = $(e.target);
    var title = $('#title').val();
    var start = $('#StartTime').val();
    var end = $('#EndTime').val();
    var allDay = $("#AllDay").prop("checked");
    var description = $('#Description').val();
    var room = $('#Room').val();
    var color = $("#colorselector").val();

    // Читаем селект участников
    var objSel = document.getElementById("selectmembers");
    var memberList = getSelectedIndexes(objSel);

    if (start == "" || end == "" || title == "") {
        $('#error-message').empty();
        $('#error-message').text('Заполните все поля');
    } else if (start == end) {
        $('#error-message').empty();
        $('#error-message').text('Начало и конец не могут быть равны');
    } else if (start > end) {
        $('#error-message').empty();
        $('#error-message').text('Начало не может быть позже конца');
    } else {

        var eventModel = {
            Title: title,
            StartTime: start,
            EndTime: end,
            AllDay: allDay,
            Description: description,
            Color: color,
            RoomId: room,
            MemberList: memberList
        };
        $.ajax({
            url: btn.data('url'),
            type: 'POST',
            data: JSON.stringify(eventModel),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            processData: false,
            success: function (result) {
                if (!result.success) {
                    $('#error-message').empty();
                    $('#error-message').text(result.message);
                } else {
                    $('#calendar').fullCalendar('refetchEvents');
                    alert("Мероприятие успешно добавлено");
                    $("#dialog").dialog("close");
                }
            },
            error: function () {
                alert('Ошибка! Добавление невозможно');
            }
        });
    }
});

$(document).on("click", "#btnDelete", function (e) {
    var btn = $(e.target);
    var eventId = $('#btnDelete').attr("cust");
    var isAdmin = confirm("Удалить мероприятие?");
    if (isAdmin) {

        $.ajax({
            type: 'POST',
            url: btn.data("url") + "?eventId=" + eventId,
            contentType: 'application/json; charset=utf-8',
            processData: false,
            success: function (data) {
                if (data.success) {
                    $('#calendar').fullCalendar('removeEvents', eventId);
                    $("#eventinfo").dialog("close");


                } else {
                    alert("При удалении возникла ошибка");
                }
            },
            error: function () {
                alert('Ошибка! Проверьте соединение!');
            }
        });
    }
});

function getSelectedIndexes(oListbox) {
    var arrIndexes = new Array;
    for (var i = 0; i < oListbox.options.length; i++) {
        if (oListbox.options[i].selected)
            arrIndexes.push(oListbox.options[i].value);
        // arrIndexes[i] = oListbox.options[i].value;
    }
    return arrIndexes;
};

$('#dialog').hide();

$('#eventinfo').hide();

$('#colorselector').colorselector();

$('#StartTime').datetimepicker({
    TimeOnlyTitle: "Выберите время",
    TimeText: "Время",
    HourText: "Часы",
    MinuteText: "Минуты",
    SecondText: "Секунды",
    CurrentText: "Сейчас",
    CloseText: "Закрыть"
});
$('#EndTime').datetimepicker({
    TimeOnlyTitle: "Выберите время",
    TimeText: "Время",
    HourText: "Часы",
    MinuteText: "Минуты",
    SecondText: "Секунды",
    CurrentText: "Сейчас",
    CloseText: "Закрыть"
});



$('#addeventform').validate({
    submitHandler: function () {
        alert('OK!');
    },

    rules: {
        Name: {
            required: true,
            minlength: 2
        }
    },
    messages: {
        Name: {
            required: "Please enter a username",
            minlength: "Your username must consist of at least 2 characters"
        }
    }
});

$(function () {
    //при нажатии на кнопку с id="save"
    $('#save').click(function () {
        //переменная formValid
        alert("ахтунг");
        var formValid = true;
        //перебрать все элементы управления input
        $('input').each(function () {
            //найти предков, которые имеют класс .form-group, для установления success/error
            var formGroup = $(this).parents('.form-group');
            //найти glyphicon, который предназначен для показа иконки успеха или ошибки
            var glyphicon = formGroup.find('.form-control-feedback');
            //для валидации данных используем HTML5 функцию checkValidity
            if (this.checkValidity()) {
                //добавить к formGroup класс .has-success, удалить has-error
                formGroup.addClass('has-success').removeClass('has-error');
                //добавить к glyphicon класс glyphicon-ok, удалить glyphicon-remove
                glyphicon.addClass('glyphicon-ok').removeClass('glyphicon-remove');
            } else {
                //добавить к formGroup класс .has-error, удалить .has-success
                formGroup.addClass('has-error').removeClass('has-success');
                //добавить к glyphicon класс glyphicon-remove, удалить glyphicon-ok
                glyphicon.addClass('glyphicon-remove').removeClass('glyphicon-ok');
                //отметить форму как невалидную
                formValid = false;
            }
        });
        //если форма валидна, то
        if (formValid) {
            //сркыть модальное окно
            $('#myModal').modal('hide');
            //отобразить сообщение об успехе
            $('#success-alert').removeClass('hidden');
        }
    });
});