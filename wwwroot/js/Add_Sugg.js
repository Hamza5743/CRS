var loadFile = function(event) {
    var imageholder = document.getElementById('output');
    var br = document.getElementById('br');
    imageholder.style["display"] = "block";
    br.style["display"] = "inline";
    for (let index = 0; index < event.target.files.length; index++) {
        var myimage = event.target.files[index];
        if ((myimage.type == "image/jpeg" || myimage.type == "image/png" || myimage.type == "image/gif") && myimage.size <= 2097152){
            imageholder.innerHTML = imageholder.innerHTML + "<img class='imgpreview' src=" + URL.createObjectURL(myimage) + ">";
        }
        else{
            var ierror = document.getElementById("ierror");
            ierror.innerHTML = "Please Provide an image of type .gif, .png, .jpg or .jpeg and size less than 2 MB!";
            var custfile = document.getElementById("customFile");
            custfile.value = "";
            imageholder.innerHTML = "";
            break;
        }
    }
};

function checkFields(){
    var dept = document.getElementById("dept");
    var content = document.getElementById("content");
    var derror = document.getElementById("derror");
    var cerror = document.getElementById("cerror");
    var numFiles = $("input:file")[0].files.length;
    derror.innerHTML = "";
    cerror.innerHTML = "";
    var submit = true;
    if (dept.value == ""){
        derror.innerHTML = "<p>Please Select A Department!</p>";
        dept.style["border"] = "1px solid red";
        submit = false;
    }
    if (content.value == ""){
        cerror.innerHTML = "<p>Please Enter Detail About Suggestion!</p>";
        content.style["border"] = "1px solid red";
        submit = false;
    }
    if (numFiles > 6){
        var ierror = document.getElementById("ierror");
        ierror.innerHTML = "You Cannot Upload More Than Six Images!";
        var custfile = document.getElementById("customFile");
        var imageholder = document.getElementById('output');
        custfile.value = "";
        imageholder.innerHTML = "";
        submit = false;
    }
    return submit;
}

function HideConfirmMsg() {
    document.getElementById("Confirmation").style.display = "none";
    document.getElementById("del").href = "#";
}