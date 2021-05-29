const socket = io('http://localhost:3000')
const messageContainer = document.getElementById('message-container')
const messageForm1 = document.getElementById('send-container1')
const messageForm2 = document.getElementById('send-container2')
const messageForm3 = document.getElementById('send-container3')
const messageForm4 = document.getElementById('send-container4')
const messageInput = document.getElementById('message-input')

const name = prompt('What is your name?')
appendMessage('you joined')
socket.emit('new-user', name)

socket.on('chat-message', data => {
    console.log(data)
    appendMessage(`${data.name}: ${data.message}`)
})

socket.on('user-connected', name => {
    console.log(name)
    appendMessage(`${name} connected`) 
})

socket.on('user-disconnected', name => {
    console.log(name)
    appendMessage(`${name} disconnected`)
})

messageForm1.addEventListener('submit', e => {
    e.preventDefault()
    const message = "Hello"
    appendMessage(`You: ${message}`)
    socket.emit('send-chat-message', message)
})

messageForm2.addEventListener('submit', e => {
    e.preventDefault()
    const message = "Do you want to play?"
    appendMessage(`You: ${message}`)
    socket.emit('send-chat-message', message)
    
})
messageForm3.addEventListener('submit', e => {
    e.preventDefault()
    const message = "I won"
    appendMessage(`You: ${message}`)
    socket.emit('send-chat-message', message)
    
})
messageForm4.addEventListener('submit', e => {
    e.preventDefault()
    const message = "Yes"
    appendMessage(`You: ${message}`)
    socket.emit('send-chat-message', message)
    
})

function appendMessage(message) {
    const messageElement = document.createElement('div')
    messageElement.innerText = message
    messageContainer.append(messageElement)

}