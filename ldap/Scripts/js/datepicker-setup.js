
//Скрипт навешивает датапикеры на формы ввода 
$(document).ready(function () {
    $("input[name='StartEvent']").datetimepicker();
    $("input[name='EndEvent']").datetimepicker();
});
