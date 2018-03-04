
function loadMaterial(fun, selData) {
    var d = null;
    if (selData && selData != undefined)
        d = selData.MaterialCode
    $.ajax({
        url: "/Inquiry/GetMaterials",
        type: 'post',
        dataType: 'json',
        data: { selData: d  },
        success: function (result) {
            $('#MaterialCode').empty();
            $.each(result, function (n, value) {
                $('#MaterialCode').append('<option value=' + value.Value + '>' + value.Name + '</option>');
                if (value.IsDefault)
                    $('#MaterialCode option[value="' + value.Value + '"]').attr('selected', true);
            })

            if (fun && fun != undefined)
                fun(null,selData);
             
        }
    });
}

//加载硬度数据
function changeMaterial(fun, selData) {
    var d = null;
    if (selData && selData != undefined)
        d = selData.Hardness
    //硬度
    $.ajax({
        url: "/Inquiry/GetMaterialData",
        type: 'post',
        dataType: 'json',
        data: { MaterialCode: $('#MaterialCode').val(), getType: 0, selData:d },
        success: function (result) {
            $('#Hardness').empty();
            $.each(result, function (n, value) {
                $('#Hardness').append('<option value=' + value.Value + '>' + value.Name + '</option>');
                if (value.IsDefault)
                    $('#Hardness option[value="' + value.Value + '"]').attr('selected', true);
            })

            if (fun && fun != undefined)
                fun(selData); 
        }
    });
}

function changeHardness(selData) {
    changeMaterial1(selData);

    changeMaterial2(selData);
    changeColor(selData);
}

function changeMaterial1(selData) {
    var d = null;
    if (selData && selData != undefined)
        d = selData.Material1
    //材料物性
    $.ajax({
        url: "/Inquiry/GetMaterialData",
        type: 'post',
        dataType: 'json',
        data: {
            MaterialCode: $('#MaterialCode').val(), Hardness: $('#Hardness').val(), getType: 1,
            selData: d
        },
        success: function (result) {
            $('#Material1').empty();
            $.each(result, function (n, value) {
                $('#Material1').append('<option value=' + value.Value + '>' + value.Name + '</option>');
                if (value.IsDefault)
                    $('#Material1 option[value="' + value.Value + '"]').attr('selected', true);
            })
        }
    });
}

function changeMaterial2(selData) {
    var d = null;
    if (selData && selData != undefined)
        d = selData.Material2
    //材料物性
    $.ajax({
        url: "/Inquiry/GetMaterialData",
        type: 'post',
        dataType: 'json',
        data: {
            MaterialCode: $('#MaterialCode').val(), Hardness: $('#Hardness').val(), getType: 2,
            selData:d
        },
        success: function (result) {
            $('#Material2').empty();
            $.each(result, function (n, value) {
                $('#Material2').append('<option value=' + value.Value + '>' + value.Name + '</option>');
                if (value.IsDefault)
                    $('#Material2 option[value="' + value.Value + '"]').attr('selected', true);
            })
        }
    });
}

function changeColor(selData) {
    var d = null;
    if (selData && selData != undefined)
        d = selData.Color
    //颜色
    $.ajax({
        url: "/Inquiry/GetMaterialData",
        type: 'post',
        dataType: 'json',
        data: {
            MaterialCode: $('#MaterialCode').val(), Hardness: $('#Hardness').val(), getType: 3,
            selData: d
        },
        success: function (result) {
            $('#Color').empty();
            $.each(result, function (n, value) {
                $('#Color').append('<option value=' + value.Value + '>' + value.Name + '</option>');


                if (value.IsDefault)
                    $('#Color option[value="' + value.Value + '"]').attr('selected', true);
            })
        }
    });

} 


function loadbaseHoles(fun) {
    $.ajax({
        url: "/material/LoadBaseHoles",
        type: 'post',
        dataType: 'json',
        //data: { MaterialCode: $('#MaterialCode').val(), getType: 0 },
        success: function (result) {
            $('#SizeC').empty();
            $.each(result, function (n, value) {
                $('#SizeC').append('<option value=' + value.SizeC + ' holes=' + value.HoleCount+'>' + value.SizeC + '</option>');
            })

            if (fun)
                fun();

        }
    });
}

function changeSize() { 
    //改变size to change hole
    var size = $('#SizeC').val();
    var holes = $('#SizeC').find('option[value=' + size + ']').attr('holes');
    var result = Number($('#Rate').val()) * Number(holes);
    $('#HoleCount').val(Math.round( result)); 
}