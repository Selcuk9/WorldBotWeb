const btnAdd = document.getElementById('btn-add');
const listBots = document.querySelector('.list-bots');
const tokenInput = document.getElementById('token');
const listUl = document.querySelector('.list-bots');
const url = 'https://localhost:44352/Telegram/GetBot';

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
