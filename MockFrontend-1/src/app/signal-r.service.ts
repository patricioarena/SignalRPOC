import { Injectable, isDevMode } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { BehaviorSubject } from 'rxjs';
import { ExpectedMessage } from './expected.messages';
import { environment } from '../environments/environment';
import { ServerMethod } from './server.method';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  public connectionId$ = new BehaviorSubject(null)

  private userId: number = 12345;
  private username: string = "alice";
  private picture: string = "https://i.pinimg.com/originals/35/0e/dd/350edd537688ad50ea3c5615e02ba84e.jpg"
  private actualUrl: string = "http://localhost:4200/"

  private connectionId: string;
  private hubConnection: signalR.HubConnection;

  public startConnection = () => {
    this.hubConnectionBuild();

    this.hubConnection.start()
      .then(() => isDevMode() && console.log("Connection started"))
      .catch(err => console.error("Error while starting connection: " + err));

    this.hubConnection.on(ExpectedMessage.welcome, (data) => {
      isDevMode() && console.log(`Welcome: ${JSON.stringify(data)}`);
      this.connectionId = data.connectionId;
      this.connectionId$.next(this.connectionId);

      const payload = {
        userId: this.userId,
        username: this.username,
        connectionId: this.connectionId,
        url: this.actualUrl,
        picture: this.picture
      }

      this.notifyConnection(payload);

    });

    this.hubConnection.on(ExpectedMessage.received_data, (data) => {
      isDevMode() && console.log({
        data: JSON.stringify(data)
      })
    });

    this.hubConnection.on(ExpectedMessage.show_notification, () => {
      this.showNotification();
    });
  }

  private hubConnectionBuild() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}SignalHub?userId=${this.userId}`)
      .build();
  }

  public notifyConnection(payload: any) {
    this.hubConnection.invoke(ServerMethod.notify_connection, payload)
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
