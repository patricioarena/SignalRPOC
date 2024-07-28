import { Injectable, isDevMode } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { ExpectedMessage } from './expected.messages';


@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private userId: number = 12345;
  private username: string = "alice";
  private picture: string = "https://i.pinimg.com/originals/35/0e/dd/350edd537688ad50ea3c5615e02ba84e.jpg"
  private actualUrl: string = "http://localhost:4200/"

  private connectionId: string;
  private hubConnection: signalR.HubConnection;

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`https://localhost:44385/SignalHub?userId=${this.userId}`)
      .build();

    this.hubConnection.start()
      .then(() => isDevMode() && console.log("Connection started"))
      .catch(err => console.error("Error while starting connection: " + err));

    this.hubConnection.on(ExpectedMessage.welcome, (data) => {
      isDevMode() && console.log(`Welcome: ${JSON.stringify(data)}`);
      this.connectionId = data.connectionId;

      const payload = {
        userId: this.userId,
        username: this.username,
        connectionId: this.connectionId,
        url: this.actualUrl,
        picture: this.picture
      }

      this.notifyConnection(payload);

    });


    this.hubConnection.on('ReceivePayloadResponse', (data) => {
      isDevMode() && console.log({
        data: JSON.stringify(data)
      })
    });

    this.hubConnection.on(ExpectedMessage.show_notification, () => {
      this.showNotification();
    });
  }

  public notifyConnection(payload: any) {
    this.hubConnection.invoke('NotifyConnection', payload)
      .catch(err => console.error(err));
  }

  private showNotification() {
    var notification = new Notification(this.username, {
      body: 'You clicked on the button!',
      icon: 'https://i.pinimg.com/originals/35/0e/dd/350edd537688ad50ea3c5615e02ba84e.jpg'
    });
    setTimeout(function () { notification.close(); }, 5000);
  }
}
