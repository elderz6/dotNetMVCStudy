var dataTable;
$(document).ready(() => {
    loadDataTable();
});

loadDataTable = () => {
    dataTable = $('#DT_load').DataTable({
        'ajax': {
            'url': '/books/getall',
            'type': 'GET',
            'datatype': 'JSON'
        },
        'columns': [
            { 'data': 'name', 'width': '20% ' },
            { 'data': 'author', 'width': '20% ' },
            { 'data': 'isbn', 'width': '20% ' },
            {
                'data': 'id',
                'render': (data) => {
                    return `<div class="text-center">
                                <a  href="/books/Upsert?id=${data}" 
                                    class="btn btn-success text-white" 
                                    style="cursor:pointer;width:100px">
                                Edit
                                </a>
                                &nbsp;
                                <a  onclick=Delete("/books/Delete?id="+${data})
                                    class="btn btn-danger text-white" 
                                    style="cursor:pointer;width:100px">
                                Delete
                                </a>
                            </div>`
                }, 'width': '30%'
            }
        ], 'language': {
            'emptyTable': 'no data found'
        }, 'width': '100%'
    })
}

Delete = (url) => {
    swal({
        title: 'Are you sure?',
        text: 'please confirm',
        icon: 'warning',
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete)
            $.ajax({
                type: 'delete',
                url: url,
                success: (data) => {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else toastr.error(data.message);
                }
            })
    });
}