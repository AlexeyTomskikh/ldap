//function OnSuccess(data) {
//    var results = $('#results'); // получаем нужный элемент
//    results.empty(); //очищаем элемент
//    results.append('<li>' + data + '</li>'); // добавляем данные в список

//}

//function showError(container, errorMessage) {
//    container.className = 'error';
//    //var msgElem = document.createElement('span');
//    var msgElem = document.createElement('div');
//    msgElem.className = "error-message";
//    msgElem.innerHTML = errorMessage;
//    container.appendChild(msgElem);
//}

//function showSuccessMessage() {

//    document.getElementById('resultsWWw').innerHTML = '';
//    document.getElementById('resultsWWw').innerHTML = 'Мероприятие успешно добавлено';
//    //showError(document.getElementById('resultsWWw'), ' Мероприятие успешно добавлено');
//}


//function resetError(container) {
//    container.className = '';
//    if (container.lastChild.className == "error-message") {
//        container.removeChild(container.lastChild);
//    }
//}

//function AddEvent(form) {

//    var elems = form.elements;
//    //var date1 = new Date(elems.StartEvent.value).getTime();
//    //var date2 = new Date(elems.EndEvent.value).getTime();
//    var date1 = $('#StartEvent').val();
//    var date2 = $('#EndEvent').val();

//    if (!elems.NameEvent.value || !elems.StartEvent.value || (!elems.EndEvent.value) || (date1 > date2)) {

//        resetError(elems.NameEvent.parentNode);
//        if (!elems.NameEvent.value) {
//            showError(elems.NameEvent.parentNode, 'Укажите название мероприятия');
//        }

//        resetError(elems.StartEvent.parentNode);
//        if (!elems.StartEvent.value) {
//            showError(elems.StartEvent.parentNode, 'Укажите время начала');
//        }

//        resetError(elems.EndEvent.parentNode);
//        if (!elems.EndEvent.value) {
//            showError(elems.EndEvent.parentNode, 'Укажите время окончания');
//        }


//        if (date1 > date2) {
//            showError(elems.EndEvent.parentNode, 'Начало не может быть раньше конца');
//        }


//    } else if ((elems.StartEvent.value == elems.EndEvent.value)) {
//        resetError(elems.StartEvent.parentNode);
//        resetError(elems.EndEvent.parentNode);
//        // showError(elems.StartEvent.parentNode, 'Начало не может совпадать с временем окончания');
//        // showError(elems.EndEvent.parentNode, ' Начало не может совпадать с временем окончания');
//        document.getElementById('resultsWWw').innerHTML = '';
//        showError(document.getElementById('resultsWWw'), ' Начало не может совпадать с временем окончания');
//        //resetError(document.getElementById('resultsWWw'));
//        //var er = document.getElementById('resultsWWw');
//        //showError(er, ' Начало не может совпадать с временем окончания');
//    } else {
//        resetError(elems.NameEvent.parentNode);
//        resetError(elems.StartEvent.parentNode);
//        resetError(elems.EndEvent.parentNode);

//        //Читаем данные с формы
//        var formData = {
//            NameEvent: $('#NameEvent').val(),
//            StartEvent: $('#StartEvent').val(),
//            EndEvent: $('#EndEvent').val()
//        }




//    }



//}