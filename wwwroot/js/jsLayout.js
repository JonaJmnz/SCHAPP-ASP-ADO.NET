document.addEventListener('DOMContentLoaded', function () {    
    //aqui se muestra el contenido del login en la ventana modal
    $('#loginModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget); // Button that triggered the modal
        var url = button.data('url'); // Extract the action URL from the data-url attribute

        var modal = $(this);
        $.get(url, function (data) {
            modal.find('.modal-body').html(data);// Load the returned HTML into the modal body
        });
        //evento para el click del btnLogin
        document.addEventListener('submit', function (event) {
            if (event.target.id === 'loginForm') {
                event.preventDefault();
                console.log("funciono");
                $.ajax({
                    type: 'POST',
                    url: 'Account/Login',
                    data: {
                        Username: $('#Username').val(),
                        Password: $('#Password').val(),
                        RememberMe: $('#RememberMe').is(':checked')
                        },
                    success: function (response) {
                        if (response === "Exito") {
                            window.location.href = '/Home/Home';
                        } else {
                            var modalBody = document.getElementById('modalBodyContent');
                            modalBody.innerHTML = response;   
                        }                     
                    }
                });
            } 
        });         
    });

    //en esta parte se habilitan o deshabilitan controles dependiendo si está logueado o no
    var txtUsuario = document.getElementById("txtUsuario");
    var opHome = document.getElementById("opHome");
    var opReg = document.getElementById("opReg");
    var opLog = document.getElementById("opLog");
    var btnSalir = document.getElementById("btnSalir");
    var idTitulo = document.getElementById("idTitulo");
    if (txtUsuario.textContent != "Usuario:") {
        //Logeado
        opLog.classList.add("d-none");
        opReg.classList.add("d-none");
        btnSalir.classList.remove("d-none");
        opHome.classList.remove("d-none");
        txtUsuario.classList.remove("d-none");
    } else {
        //sin logear
        if (idTitulo != null) {
            opReg.classList.add("d-none");
            opLog.classList.add("d-none");
        } else {
            opReg.classList.remove("d-none");
            opLog.classList.remove("d-none");
        }
        btnSalir.classList.add("d-none");
        txtUsuario.classList.add("d-none");
    }
});