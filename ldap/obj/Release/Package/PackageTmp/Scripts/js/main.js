

$('#calendar').fullCalendar({
    theme: true,
    dayClick: function (date, jsEvent, view) {
        $('#calendar').fullCalendar('changeView', 'agendaDay', date);
    },
    eventLimit: true,
    displayEventTime: true,
    displayEventEnd: true,
    BusinessHours: false,
    //selectable: true,
    //editable: true,
    customButtons: {
        myCustomButton: {
            text: 'Добавить мероприятие',
            click: function () {
                $("#popup1").show();
            }

        },
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
            url: '/Logic/GetCalendarEvents/',
            //url: '/Scheduler/Logic/GetCalendarEvents/',

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
    eventRender: function (event, element) {


    },

    timeFormat: 'HH:mm',

    // Клик по событию открывает всплывающее окно
    eventClick: function (calEvent, jsEvent, view) {
        $('#btnDelete').attr("cust", calEvent.id);
        $('#eventinfo #when').text(calEvent.start.format("D MMM HH:mm") + " - " + calEvent.end.format("HH:mm"));

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


function CreateEvent() {

    var title = $('#NameEvent').val();
    var date1 = $('#StartEvent').val();
    var date2 = $('#EndEvent').val();

    if (date1 == "" || date2 == "" || title == "") {
        $('#error-message').empty();
        $('#error-message').text('Заполните все поля');
    } else if (date1 == date2) {
        $('#error-message').empty();
        $('#error-message').text('Начало и конец не могут быть равны');
    } else if (date1 > date2) {
        $('#error-message').empty();
        $('#error-message').text('Начало не может быть позже конца');
    } else {
        var formData = {
            NameEvent: $('#NameEvent').val(),
            StartEvent: $('#StartEvent').val(),
            EndEvent: $('#EndEvent').val()
        };
        $.ajax({
            url: '/Scheduler/Logic/AddNewEvent',
            type: 'POST',
            data: JSON.stringify(formData),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            processData: false,
            success: function (result) {
                if (!result.success) {
                    $('#error-message').empty();
                    $('#error-message').text(result.message);
                } else {
                    $('#calendar').fullCalendar('refetchEvents');
                    $('.form').val("");
                    $('.form').empty();
                    $('#error-message').empty();
                    alert("Мероприятие успешно добавлено");
                    $("#dialog").dialog("close");
                }
            },
            error: function () {
                alert('Ошибка! Добавление невозможно');
            }
        });
    }
}


$('#StartEvent').datetimepicker({
    TimeOnlyTitle: "Выберите время",
    TimeText: "Время",
    HourText: "Часы",
    MinuteText: "Минуты",
    SecondText: "Секунды",
    CurrentText: "Сейчас",
    CloseText: "Закрыть"
});
$('#EndEvent').datetimepicker({
    TimeOnlyTitle: "Выберите время",
    TimeText: "Время",
    HourText: "Часы",
    MinuteText: "Минуты",
    SecondText: "Секунды",
    CurrentText: "Сейчас",
    CloseText: "Закрыть"
});

$('#dialog').hide();



    $('#btnDelete').click(function() {

        var eventId = $('#btnDelete').attr("cust");
        var isAdmin = confirm("Удалить мероприятие?");
        if (isAdmin) {

            $.ajax({
                type: 'POST',
                //url: '/Scheduler/Logic/RemoveEvent?eventId=' + eventId,
                url: '/Logic/RemoveEvent?eventId=' + eventId,
                contentType: 'application/json; charset=utf-8',
                processData: false,
                success: function(data) {
                    if (data.success) {
                        $('#calendar').fullCalendar('removeEvents', eventId);
                        $("#eventinfo").dialog("close");


                    } else {
                        alert("При удалении возникла ошибка");
                    }

                },
                error: function() {
                    alert('Ошибка! Проверьте соединение!');
                }
            });
        }
    });
    $('#eventinfo').hide();