function showWnd() {
    $('#dialogDiv')
        .load('/Logic/SomeAction/')
        .dialog({autoOpen: true, modal: true, width: 400, height:300, title: 'Добавить новое мероприятие', resizable: false });
};


