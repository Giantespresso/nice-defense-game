var current = "Carrot";

function DisplayInfo(){
    var x = document.getElementById("gamerBoy");
    x.classList.toggle("hidden");
}

function SetActive(name){
    if (name === current)
        return;

    document.getElementById(current).classList.toggle("hidden");
    document.getElementById(name).classList.toggle("hidden");
    current = name;
}