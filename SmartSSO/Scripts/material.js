
function loadMaterial(fun) {
    $.ajax({
        url: "/Inquiry/GetMaterials",
        type: 'post',
        dataType: 'json',
        //data: { MaterialCode: $('#MaterialCode').val(), getType: 0 },
        success: function (result) {
            $('#MaterialCode').empty();
            $.each(result, function (n, value) {
                $('#MaterialCode').append('<option value=' + value.Value + '>' + value.Text + '</option>');
            })

            if (fun)
                fun();
             
        }
    });
}


function changeMaterial(fun) {
    //硬度
    $.ajax({
        url: "/Inquiry/GetMaterialData",
        type: 'post',
        dataType: 'json',
        data: { MaterialCode: $('#MaterialCode').val(), getType: 0 },
        success: function (result) {
            $('#Hardness').empty();
            $.each(result, function (n, value) {
                $('#Hardness').append('<option value=' + value.Value + '>' + value.Name + '</option>');
            })

            if (fun)
                fun(); 
        }
    });
}

function changeMaterial1() {
    //材料物性
    $.ajax({
        url: "/Inquiry/GetMaterialData",
        type: 'post',
        dataType: 'json',
        data: {
            MaterialCode: $('#MaterialCode').val(), Hardness: $('#Hardness').val(), getType: 1
        },
        success: function (result) {
            $('#Material1').empty();
            $.each(result, function (n, value) {
                $('#Material1').append('<option value=' + value.Value + '>' + value.Name + '</option>');
            })
        }
    });
}

function changeMaterial2() {
    //材料物性
    $.ajax({
        url: "/Inquiry/GetMaterialData",
        type: 'post',
        dataType: 'json',
        data: {
            MaterialCode: $('#MaterialCode').val(), Hardness: $('#Hardness').val(), getType: 2
        },
        success: function (result) {
            $('#Material2').empty();
            $.each(result, function (n, value) {
                $('#Material2').append('<option value=' + value.Value + '>' + value.Name + '</option>');
            })
        }
    });
}

function changeColor() {
    //颜色
    $.ajax({
        url: "/Inquiry/GetMaterialData",
        type: 'post',
        dataType: 'json',
        data: {
            MaterialCode: $('#MaterialCode').val(), Hardness: $('#Hardness').val(), getType: 3
        },
        success: function (result) {
            $('#Color').empty();
            $.each(result, function (n, value) {
                $('#Color').append('<option value=' + value.Value + '>' + value.Name + '</option>');
            })
        }
    });

} 