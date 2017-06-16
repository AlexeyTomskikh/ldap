
//Перезагружает календарь
function reloadCalendar(year, month, day) {
    $('#calendar').load('_getCalendar', { 'year': year, 'month': month, 'day': day });
}

//Загружает список событий в этот день
function loadEventsOfDay(currentDate) {
    reloadSchedule(currentDate)
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
