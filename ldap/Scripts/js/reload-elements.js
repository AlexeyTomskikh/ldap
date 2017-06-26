
//Перезагружает календарь
function reloadCalendar(year, month, day) {
    $('#mycalendar').load('_getCalendar', { 'year': year, 'month': month, 'day': day });
}

//Загружает список событий в этот день
function loadEventsOfDay(currentDate) {
    //reloadSchedule(currentDate);
    switchView(currentDate);
    $('#eventsofday').load('_getEventsOfDay', { 'currentDate': currentDate });

}
// отображает список названий мероприятий в табличке
function ShowEventsInTable(nameEvent) {
    $('#eventsofday').load('_getCalendar', { 'currentDate': nameEvent });
}

//Перегружает расписание на день
function reloadSchedule(currentDate) {
    //alert("Привет из функции reloadSchedule")
    $('#schedule').load('_daySchedule', { 'currentDate': currentDate });

}

//Функция подгружает данные в  FullCalendar на переданную дату
function reloadFullCalendar(currentDate) { // 03.06.2017 0:00:00

    //var month = currentDate.timeStamp;
    // var unixtime = new Date(currentDate).getUnixTime();

    // 1ый способ не рабочий. выдаёт Mon Mar 06 2017 00:00:00 GMT+0700 . Прибавляет 3 дня
    // var date = new Date(currentDate);
    // alert(date); 

    //2-ой способ через moment. без второго параметра выдаёт timestamp только до 12-го числа
    var timestamp = moment(currentDate, "DD.MM.YYYY"); // преобразуем дату по которой кликнули в TimeStampt
    
    //Проверка
    alert("2-ой способ через moment = " + timestamp);
    var date = new Date();
    date.setTime(timestamp);
    alert(date.toDateString());
    
    $('#calendar').fullCalendar('gotoDate', timestamp)
    
}


function switchView(currentDate) { // 03.06.2017 0:00:00

    var timestamp = moment(currentDate, "DD.MM.YYYY"); // преобразуем дату по которой кликнули в TimeStampt
    $('#calendar').fullCalendar('changeView', 'agendaDay', timestamp);
}