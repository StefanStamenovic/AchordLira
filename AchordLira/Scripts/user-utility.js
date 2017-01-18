window.onload = function () {


    $('#addGenreModal').on('hidden.bs.modal', function () {
        $('#genreExistsError').hide();
    });

    $('#genreExistsError').hide();

    if (window.location.hash == '#addGenreModal') {
        $('#addGenreModal').modal();
        $('#genreExistsError').show();
    }

    $('#deleteGenreModal').on('hidden.bs.modal', function () {
        $('#genreNotExistError').hide();
    });

    $('#genreNotExistError').hide();

    if (window.location.hash == '#deleteGenreModal') {
        $('#deleteGenreModal').modal();
        $('#genreNotExistError').show();
    }



    $('#addArtistModal').on('hidden.bs.modal', function () {
        $('#artistExistsError').hide();
    });

    $('#artistExistsError').hide();

    if (window.location.hash == '#addArtistModal') {
        $('#addArtistModal').modal();
        $('#artistExistsError').show();
    }



    $('#deleteArtistModal').on('hidden.bs.modal', function () {
        $('#artistNotExistError').hide();
    });

    $('#artistNotExistError').hide();

    if (window.location.hash == '#deleteArtistModal') {
        $('#deleteArtistModal').modal();
        $('#artistNotExistError').show();
    }


    $(document).on('shown.bs.tab', 'a[data-toggle="tab"]', function () {
        $.ajax({
            type: "POST",
            url: '/User/RefreshSubmissions',
            contentType: "application/json; charset=utf-8",
            async: true,
            success: function () {
                $('#notificationsCount').hide();
            },
            error: function () { alert('Error') },
            failure: function () { alert('Failure') }
        });
    })

    if ($('#notificationsCount').html() == '0') {
        $('#notificationsCount').hide();
    }
};


