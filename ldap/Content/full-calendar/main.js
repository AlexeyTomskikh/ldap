//$(document).ready(function () {

//    // page is now ready, initialize the calendar...

//    $('#calendar').fullCalendar({
//    })
//});

$('#calendar').fullCalendar({
    theme: true,
    // оБработчик клика по дню
    dayClick: function(date, jsEvent, view) {
        var clickDAte = date.format();
        $('#start').val(clickDAte); // устанавливаем в инпут автоматом дату по которой кликнули
        $('#dialog').dialog('open'); // открытие окна добавления события

    },
    header: {
        left: 'prev,next today',    // отображаемые кнопки
        center: 'title',
        right: 'month, agendaDay'
    
    },
    defaultView: 'month',
    
    events: function (start, end, timezone, callback) {

        var date = new Date();
        $.ajax({
            url: '/Logic/GetCalendarEvents/',
            dataType: 'json',
            data: {
                start: start.unix(),
                end: end.unix()
            },
            success: function (doc) {
                // The following code is taken from documentation.
                var events = doc;
                callback(events);
            }
        });
    }

    //events: [
    //    {
    //        title: 'Событие 1',
    //        start: '2017-06-26T12:00:00',
    //        end: '2017-06-27T10:00:00',
    //    }
    //]
});

$('#dialog').dialog({
    autoOpen: false,
    show: {
        effect: 'drop',
        duration: 500
    },
    hide: {
        effect: 'clip',
        duration: 500
    }
});

$('.datepicker').datepicker({
    dateFormat: "yy-mm-dd"
});

$('#calendar').fullCalendar({
    events: '/GetCalendarEvents'
});