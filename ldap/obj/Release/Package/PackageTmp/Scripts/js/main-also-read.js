
$('#calendarAlsoRead').fullCalendar({
    theme: true,
    dayClick: function (date, jsEvent, view) {
        $('#calendarAlsoRead').fullCalendar('changeView', 'agendaDay', date);
    },
    eventLimit: true,
    displayEventTime: true,
    displayEventEnd: true,
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
            url: '/Scheduler/Logic/GetCalendarEvents/',
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
    timeFormat: 'HH:mm'
});

