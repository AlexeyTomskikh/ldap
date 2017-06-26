function OnSuccess(data) {
    var results = $('#results'); // получаем нужный элемент
    results.empty(); //очищаем элемент
    results.append('<li>' + data + '</li>'); // добавляем данные в список

}



function AddEventFull(form) {

    var elems = form.elements;
    var date1 = $('#start').val();
    var date2 = $('#end').val();

   
        //Читаем данные с формы
        var formData = {
            NameEvent: $('#title').val(),
            StartEvent: $('#start').val(),
            EndEvent: $('#end').val()
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
                   
                    //alert("К сожалению указанное время уже занято. Попробуйте выбрать другое")
                } else {

                    alert("Мероприятие успешно добавлено")
                }
            },
            error: function () {
                alert('Ошибка! Добавление невозможно. Проверьте соединение с интернетом!');
            }

        });
    }


