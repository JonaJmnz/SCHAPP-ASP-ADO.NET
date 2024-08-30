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
    const inpInContraseña = document.getElementById('inpInContraseña');
    const inpInContraseña2 = document.getElementById('inpInContraseña2');
    const inpOtrServ = document.getElementById('inpInContraseña2');

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
        if (this.value === "2-3") {
            inpOtrServ.classList.remove("d-none");
        } else {
            inpOtrServ.classList.add("d-none");
        }
    });

    //--------------Enviar los datos del registro
    document.getElementById("btnRegistrar").addEventListener("click", function () {
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

    var opReg = document.getElementById("opReg");
    opReg.classList.add("d-none");
    console.log("opReg se debe ocultar")
}); 