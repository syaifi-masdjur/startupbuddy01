let oTable;

$(document).ready(function () {
    bindTable();
});

function bindTable() {
    if ($.fn.DataTable.isDataTable('#tableData')) {
        oTable.draw();
    } else {
        let id = $('#id').val();
        //let base = $('#_base').val();
        let link = "/Home/OpenData";
        oTable = $('#tableData').DataTable({
            "bProcessing": true,
            "serverSide": true,
            "sAjaxSource": link,
            "searching": true,
            "filter":true,
            "fixedColumns": { leftColumns: 1 },
            "fnServerData": function (sSource, aoData, fbCallback) {
                //console.log(aoData);
                $.ajax({
                    type: "POST",
                    data: { id, param: aoData },
                    url: sSource,
                    success: fbCallback
                });
            },
            "order": [0, "asc"],
            "columns": [
                { "data": "id", "name": "id" },
                {"data": "name", "name": "name"},
                { "data": "email", "name": "email" },
                {
                    "data": "photo", "name": "photo",
                    "render": function (data, type, dta, meta) {
                        if (data === null) {
                            return ``;
                        };
                        if (data.length !== 0) {
                            return `<a href="javascript:;" class="text-success bold text-sm" onclick=preview(${dta.id})>${data}</a>`;
                        } else {
                            return ``;
                        }
                    } },
                { "data": "gender", "name": "gender" },
                { "data": "age", "name": "age" },
                { "data": "address", "name": "address" },
                { "data": "position", "name": "position" },
                {
                    "data": "id", "name": "id", "render":function (dta) {
                        return `<a href="javascript:;" class="text-success bold text-sm" onclick=edit(${dta})>Edit</a> | <a href="javascript:;" class="text-danger bold text-sm" onclick=remove(${dta})>Remove</a> `;
                    }
                },
            ]
        });
    }
}

function add() {
    //var base = $("#_base").val();
    var link = "/Home/add"
    $("#dgModal").remove();
    $.get(link, function (dta) {
        $("body").append(dta);
        $("#dgModal").modal({
            backdrop: 'static',
            keyboard: true,
            show: true
        });

        var frm = $('#frmAdd');
        $('.s2').select2({ theme: 'bootstrap4' });
        $.validator.unobtrusive.parse(frm);
        frm.bind('submit', function (ev) {
            let token = $('input[name="__RequestVerificationToken"]', frm).val();
            let fileUpload = $("#file").get(0);
            let files = fileUpload.files[0];
            var formData = new FormData();
            formData.append('file', files);
            formData.append('name', $('#name').val());
            formData.append('email', $('#email').val());
            formData.append('gender', $('input[name="gender"]:checked').val());
            formData.append('age', $('#age').val());
            formData.append('address', $('#address').val());
            formData.append('position', $('#position').val());
            formData.append('__RequestVerificationToken', token);

            $.ajax({
                type: "POST",
                url: link,
                data: formData,
                contentType: false,
                processData: false,
                xhr: function () {
                    let xhr = new window.XMLHttpRequest();
                    xhr.upload.addEventListener("progress", function (e) {
                        if (e.lengthComputable) {
                            var percentage = (e.loaded / e.total) * 100;
                            $('#progress').attr('aria-valuenow', percentage).css('width', percentage + '%');
                        }
                    });
                    return xhr;
                },
                success: function (res) {
                    if (res.isSuccess === true) {
                        $('.modal-backdrop').hide(); // for black background
                        $('body').removeClass('modal-open'); // F/or scroll run
                        $('#dgModal').modal('hide');
                        $("#dgModal").remove();
                        toastr.success('Data is Saved')
                        bindTable();
                    } else {
                        $('.modal-backdrop').hide(); // for black background
                        $('body').removeClass('modal-open'); // For scroll run
                        $('#dgModal').modal('hide');
                        $("#dgModal").remove();
                        toastr.error(res.Messages)
                    }
                }
            });
            ev.preventDefault();
            ev.stopImmediatePropagation();
            return false;
        })
    })
}

function edit(id) {
    //let base = $('#_base').val();

    let link = "/Home/Edit"
    $("#dgModal").remove();
    $.get(link, { id}, function (dta) {
        $("body").append(dta);
        $("#dgModal").modal('show');
        let frm = $('#frmEdit');
        $('.s2').select2({ theme: 'bootstrap4' });
        $.validator.unobtrusive.parse(frm);
        frm.bind('submit', function (ev) {
            let token = $('input[name="__RequestVerificationToken"]', frm).val();
            let fileUpload = $("#file").get(0);
            let files = fileUpload.files[0];
            var formData = new FormData();
            formData.append('file', files);
            formData.append('id', $('#id').val());
            formData.append('name', $('#name').val());
            formData.append('email', $('#email').val());
            formData.append('gender', $('input[name="gender"]:checked').val());
            formData.append('age', $('#age').val());
            formData.append('address', $('#address').val());
            formData.append('position', $('#position').val());
            formData.append('__RequestVerificationToken', token);
            $.ajax({
                type: "POST",
                url: link,
                data: formData,
                contentType: false,
                processData: false,
                xhr: function () {
                    let xhr = new window.XMLHttpRequest();
                    xhr.upload.addEventListener("progress", function (e) {
                        if (e.lengthComputable) {
                            var percentage = (e.loaded / e.total) * 100;
                            $('#progress').attr('aria-valuenow', percentage).css('width', percentage + '%');
                        }
                    });
                    return xhr;
                },
                success: function (res) {
                    if (res.isSuccess === true) {
                        $('.modal-backdrop').hide(); // for black background
                        $('body').removeClass('modal-open'); // F/or scroll run
                        $('#dgModal').modal('hide');
                        $("#dgModal").remove();
                        toastr.success('Data is Saved')
                        bindTable();
                    } else {
                        $('.modal-backdrop').hide(); // for black background
                        $('body').removeClass('modal-open'); // For scroll run
                        $('#dgModal').modal('hide');
                        $("#dgModal").remove();
                        toastr.error(res.Messages)
                    }
                }
            });
            ev.preventDefault();
            ev.stopImmediatePropagation();
            return false;
        })
    })
}

function editX(id) {
    //let base = $('#_base').val();

    let link = "/Home/Edit"
    $("#dgModal").remove();
    $.get(link, { id }, function (dta) {
        $("body").append(dta);
        $("#dgModal").modal('show');
        let frm = $('#frmEdit');
        $('.s2').select2({ theme: 'bootstrap4' });
        $.validator.unobtrusive.parse(frm);
        frm.bind('submit', function (ev) {
            $.ajax({
                type: frm.attr('method'),
                url: frm.attr('action'),
                data: frm.serialize(),
                success: function (res) {
                    if (res.isSuccess === true) {
                        $('.modal-backdrop').hide(); // for black background
                        $('body').removeClass('modal-open'); // F/or scroll run
                        $('#dgModal').modal('hide');
                        $("#dgModal").remove();
                        toastr.success('Data is Saved')
                        bindTable();
                    } else {
                        $('.modal-backdrop').hide(); // for black background
                        $('body').removeClass('modal-open'); // For scroll run
                        $('#dgModal').modal('hide');
                        $("#dgModal").remove();
                        toastr.error(res.Messages)
                    }
                }
            });
            ev.preventDefault();
            ev.stopImmediatePropagation();
            return false;
        })
    })
}

function remove(idx) {
    //Getting current data
    //tampil swal

    Swal.fire({
        type: 'question',
        title: 'Are You Sure',
        html:'Do you really want to delete this record?<br/> <b class="text-md">This process cannot be undone</b>',
        showDenyButton: true,
        showCancelButton: true,
        confirmButtonText: `Delete`,
        denyButtonText: `Cancel`,
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            let base = "";
            let link = base + "/Home/Delete";
            $.post(link, { id: idx }, function (res) {
                if (res.isSuccess === true) {
                    toastr.success("Record is Deleted")
                    bindTable();
                } else {
                    toastr.error("Fail to Delete")
                }
            });
        }
    })
}

function preview(id) {
    //let base = $('#_base').val();

    let link = "/Home/preview"
    $("#dgModal").remove();
    $.get(link, { id }, function (dta) {
        $("body").append(dta);
        $("#dgModal").modal('show');
    })
}