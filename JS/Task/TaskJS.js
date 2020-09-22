$(document).ready(function () {
    $('#example').DataTable({
        bFilter: false,
        bInfo: false,
        dom: 'frtlip'
    });

    $('#wizard').smartWizard();

    function onFinishCallback() {
        $('#wizard').smartWizard('showMessage', 'Finish Clicked');
        //alert('Finish Clicked');
    }
});