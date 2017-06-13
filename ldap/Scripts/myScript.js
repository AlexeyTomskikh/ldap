function showWnd() {
    $('#dialogDiv')
        .load('/Logic/SomeAction/')
        .dialog({autoOpen: true, modal: true, width: 600, height:400, title: 'Добавить новое мероприятие', resizable: false });
};