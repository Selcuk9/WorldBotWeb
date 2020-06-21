const btnAdd = document.getElementById('btn-add');
const listBots = document.querySelector('.list-bots');
const tokenInput = document.getElementById('token');
const listUl = document.querySelector('.list-bots');
const url = '/Telegram/AddBot';
const urlRun = '/Telegram/RunBot';
const urlStop = '/Telegram/StopBot';

btnAdd.addEventListener('click', async function () {
    if (tokenInput.value != "" && tokenInput.value != null) {


        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(tokenInput.value)
        }).then(response => response.json())

        listBots.innerHTML += `<li id="${listUl.childNodes.length - 1}" onclick="GetChat(event);">
                                    ${response.username}
                                </li>`
        tokenInput.value = "";
    }
});

function GetChat(event) {
    alert(event.target.id);
}


let btnRun = document.getElementById('btn-run'),
    btnStop = document.getElementById('btn-stop');
btnRun.addEventListener('click', function () {
    fetch(urlRun, {
        method: 'POST'
    });
});
btnStop.addEventListener('click', function () {
    fetch(urlStop, {
        method: 'POST'
    });
})