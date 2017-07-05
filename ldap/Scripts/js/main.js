
$('#calendar').fullCalendar({
    theme: true,
    dayClick: function (date, jsEvent, view) {
        $('#calendar').fullCalendar('changeView', 'agendaDay', date);
    },
    eventLimit: true,
    displayEventTime: true,
    displayEventEnd: true,
    BusinessHours: false,
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
    allDaySlot: false,       //убираем события длиною в день
    slotEventOverlap: false, // запрещаем пересечение событий

    //загрузчик событий в календарь
    events: function (start, end, timezone, callback) {

        $.ajax({
            url: '/Logic/GetCalendarEvents/',
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

    // Клик по событию удаляет событие
    eventClick: function (calEvent, jsEvent, view) {
        var isAdmin = confirm("Удалить мероприятие?");
        if (isAdmin) {
            var eventId = calEvent.id;
            $.ajax({
                type: 'POST',
                url: '/Logic/RemoveEvent?eventId=' + eventId,
                contentType: 'application/json; charset=utf-8',
                processData: false,
                success: function (data) {
                    if (data.success) {
                        alert("Мероприятие успешно удалено");
                        $('#calendar').fullCalendar('removeEvents', eventId);
                    } else {
                        alert("При удалении возникла ошибка");
                    }

                },
                error: function () {
                    alert('Ошибка! Проверьте соединение!');
                }
            });
        }
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
        }

        $.ajax({
            url: '/Logic/AddNewEvent',
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
