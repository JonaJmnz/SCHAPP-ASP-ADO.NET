document.addEventListener('DOMContentLoaded', () => {
    //--------------Mapeo de los inputs
    const inpNombreUsu = document.getElementById('inpNombreUsu');
    const inpNombre = document.getElementById('inpNombre');
    const inpApPaterno = document.getElementById('inpApPaterno');
    const inpApMaterno = document.getElementById('inpApMaterno');
    const inpTelefono = document.getElementById('inpTelefono');
    const inpDomicilio = document.getElementById('inpDomicilio');
    const inpFdeNacimiento = document.getElementById('inpFdeNacimiento');
    const inpEmail = document.getElementById('inpEmail');
    const inpContraseña = document.getElementById('inpContraseña');
    const inpContraseña2 = document.getElementById('inpContraseña2');
    const inpOtrServ = document.getElementById('inpOtrServ');
    const UsExistT = document.getElementById('UsExistT');
    const UsExistF = document.getElementById('UsExistF');
    const EmExistT = document.getElementById('EmExistT');
    const EmExistF = document.getElementById('EmExistF');
    const passConfirmT = document.getElementById('passConfirmT');
    const passConfirmF = document.getElementById('passConfirmF');
    const btnRegistrar = document.getElementById("btnRegistrar");
    const slctCliServ = document.getElementById("slctCliServ");

    //btnSubmitArea
    document.getElementById("btnSubmitArea").addEventListener("mouseover", function (event) {
        if (inpNombreUsu.value != "" && inpNombre.value != "" &&
            inpApPaterno.value != "" && inpApMaterno.value != "" &&
            inpTelefono.value != "" && (inpContraseña.value === inpContraseña2.value) &&
            inpContraseña.value != "" && inpContraseña2.value != "" &&
            inpDomicilio.value != "" && inpDomicilio.value != "" &&
            slctCliServ.value != "Selecciona una opción") {

            btnRegistrar.disabled = slctCliServ.value === "0" && inpOtrServ.value === "";

        } else {
            btnRegistrar.disabled = true;
        }
    });

    $('#inpTelefono').mask('(000) 000-0000'); // Aplica la máscara
    traerRoles();

    //--------------LIMITE DE INPUT DE FECHA
    // Obtener la fecha actual
    const hoy = new Date();
    // Calcular la fecha mínima (7 días antes de hoy) y máxima (7 días después de hoy)
    const fechaMin = new Date(hoy.getFullYear() - 100, hoy.getMonth(), hoy.getDate());
    const fechaMax = new Date(hoy.getFullYear() - 15, hoy.getMonth(), hoy.getDate());
    // Función para formatear la fecha a "YYYY-MM-DD"
    const formatoFecha = (fecha) => {
        const año = fecha.getFullYear();
        const mes = String(fecha.getMonth() + 1).padStart(2, '0');
        const día = String(fecha.getDate()).padStart(2, '0');
        return `${año}-${mes}-${día}`;
    };
    // Asignar las fechas mínima y máxima al input
    inpFdeNacimiento.min = formatoFecha(fechaMin);
    inpFdeNacimiento.max = formatoFecha(fechaMax);
    inpFdeNacimiento.value = formatoFecha(fechaMax);

    //--------------Inicializar tooltips de Bootstrap
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.forEach(function (tooltipTriggerEl) {
        new bootstrap.Tooltip(tooltipTriggerEl);
    });

    //--------------Evento para poder agregar otro servicio diferente
    document.getElementById("slctCliServ").addEventListener("change", function () {
        var inpOtrServ = document.getElementById("inpOtrServ");
        console.log(this.value);
        if (this.value === "0") {
            inpOtrServ.classList.remove("d-none");
        } else {
            inpOtrServ.classList.add("d-none");
        }
    });

    //--------------Enviar los datos del registro
   /* document.getElementById("btnRegistrar").addEventListener("click", function () {
        inpNombre.disbaled = true;
        inpApPaterno.disbaled = true;
        inpApMaterno.disbaled = true;
        inpDomicilio.disbaled = true;
        inpFdeNacimiento.disbaled = true;
        inpEmail.disbaled = true;
        inpInContraseña.disbaled = true;
        inpInContraseña2.disbaled = true;
        inpOtrServ.disbaled = true;

        var btnRegistrar = document.getElementById("btnRegistrar");
        var spnrRegistrar = document.getElementById("spnrRegistrar");
        btnRegistrar.classList.add("d-none");
        spnrRegistrar.classList.remove("d-none");
    });
    */
    var opReg = document.getElementById("opReg");
    opReg.classList.add("d-none");
    console.log("opReg se debe ocultar")

    //deshabilitar el submit al dar enter
    document.getElementById("registrarseForm").addEventListener("keydown", function (event) {
        if (event.key === "Enter") {
            event.preventDefault(); // Evita que el formulario se envíe
        }
    });

    //evento para validar el nombre de usuario
    document.getElementById("inpNombreUsu").addEventListener("change", function () {
        existeUsuario();
    });
    document.getElementById("inpEmail").addEventListener("change", function () {
        existeEmail();
    });
    document.getElementById("inpContraseña2").addEventListener("input", function () {
        passMatch(inpContraseña.value, inpContraseña2.value);
    });
    document.getElementById("inpContraseña").addEventListener("input", function () {
        passMatch(inpContraseña.value, inpContraseña2.value);
    });

    //REGISTRAR USUARIO
    document.getElementById("registrarseForm").addEventListener("submit", function (event) {
        // Permitir que el formulario se envíe normalmente
        // Aquí no usamos event.preventDefault() porque dejamos que el formulario siga su curso
        event.preventDefault();
        var formData = new FormData(this);

        deshabilitarForm();
        // Enviar los datos usando fetch (pero el formulario seguirá su curso)
        fetch('/Account/Registrarse', {
            method: 'POST',
            body: formData
        })
            .then(response => response.json())
            .then(data => {
                // Capturar y mostrar el mensaje de respuesta
                if (data.mensaje != null) {
                    //document.getElementById("modelError").innerText = data.mensaje;
                    mostrarAlerta(data.mensaje);
                }

                // Prevenir el recargado después de recibir la respuesta (opcional)
                event.preventDefault(); // Solo si decides no recargar la página.
            })
            .catch(error => console.error('Error:', error));

    });
    function existeUsuario() {
        $.ajax({
            type: 'POST',
            url: 'ExisteUsuario',
            data: {
                nombreUsuario: $('#inpNombreUsu').val(),
            },
            success: function (response) {
                if (response) {
                    UsExistT.classList.remove("d-none");
                    UsExistF.classList.add("d-none");
                } else {
                    UsExistT.classList.add("d-none");
                    UsExistF.classList.remove("d-none");
                }
            }
        });
    }
    function existeEmail() {
        $.ajax({
            type: 'POST',
            url: 'ExisteEmail',
            data: {
                email: $('#inpEmail').val(),
            },
            success: function (response) {
                if (response) {
                    EmExistT.classList.remove("d-none");
                    EmExistF.classList.add("d-none");
                } else {
                    EmExistT.classList.add("d-none");
                    EmExistF.classList.remove("d-none");
                }
            }
        });
    }
    function traerRoles() {
        $.ajax({
            type: 'POST',
            url: 'traerRoles',
            data: '',
            success: function (data) {
                console.log(data); // La lista de empleados
                // Obtener el optgroup por su ID
                var grupo = document.getElementById("slctCliServ");
                var valor = 1;
                data.forEach(function (rol) {
                    console.log(rol);
                    // Crear un nuevo elemento option
                    var nuevaOpcion = document.createElement("option");
                    // Establecer el texto y el valor del nuevo option
                    nuevaOpcion.text = rol;
                    nuevaOpcion.value = valor;
                    // Agregar el nuevo option al optgroup
                    grupo.appendChild(nuevaOpcion);
                    valor++;
                });
                var opOtroServicio = document.createElement("option");
                opOtroServicio.text = "Otro...";
                opOtroServicio.value = 0;
                opOtroServicio.id = "opOtrServ";
                grupo.appendChild(opOtroServicio);
            },
            error: function (err) {
                console.log("Error: ", err);
            }
        });
    }
    function passMatch(pass1, pass2) {
        console.log(pass1 + "-----" + pass2);
        if (pass1 === pass2) {
            passConfirmT.classList.remove("d-none");
            passConfirmF.classList.add("d-none");
        } else {
            passConfirmT.classList.add("d-none");
            passConfirmF.classList.remove("d-none");
        }
    }
    async function mostrarAlerta(exito) {
        var alerta = document.getElementById('alertContainer');
        var alertaHTML;
        if (exito === 'Exit') {
            alertaHTML = '<div class="alert alert-info alert-success alert-dismissible fade show" role="alert">Usuario Registrado con exito<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button></div>'
            //deshabilitarForm();

            var btnRegistrar = document.getElementById("btnRegistrar");
            var spnrRegistrar = document.getElementById("spnrRegistrar");
            btnRegistrar.classList.add("d-none");
            spnrRegistrar.classList.remove("d-none");
            alerta.innerHTML = alertaHTML;
            await esperar(4000);
            //console.log('cambie de pagina')
            window.location.href = '/Home/Index';
        } else {
            alertaHTML = '<div class="alert alert-info alert-warning alert-dismissible fade show" role="alert">Ocurrio algun error al registrar el usuario.<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button></div>';
            alerta.innerHTML = alertaHTML;
            habilitarForm();
        }
    }
    
    // Función que devuelve una promesa para esperar n milisegundos
    function esperar(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }

    function deshabilitarForm() {
        // Selecciona el formulario
        const formulario = document.getElementById('registrarseForm');

        // Itera sobre todos los elementos dentro del formulario
        Array.from(formulario.elements).forEach(element => {
            element.disabled = true;
        });
    } function habilitarForm() {
        // Selecciona el formulario
        const formulario = document.getElementById('registrarseForm');

        // Itera sobre todos los elementos dentro del formulario
        Array.from(formulario.elements).forEach(element => {
            element.disabled = false;
        });
    }
}); 