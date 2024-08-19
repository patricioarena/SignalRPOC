import { Injectable, isDevMode } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { ExpectedMessage } from './contants/expected.messages';
import { environment } from '../../../environments/environment';
import { ServerMethod } from './contants/server.method';
import { EncodeService } from "./encode-service";

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  public connectionId$ = new BehaviorSubject(null)

  private userId: number = 1234;
  private username: string = "Falice";
  private picture: string = "https://i.pinimg.com/originals/35/0e/dd/350edd537688ad50ea3c5615e02ba84e.jpg"
  private actualUrl: string = "http://localhost:4200/"

  private connectionId: string;
  private hubConnection: signalR.HubConnection;
  private queryParamUserId: string = "UserId";

  constructor(private encodeService: EncodeService) { }

  public startConnection = () => {

    this.hubConnectionBuild();

    this.hubConnection.start()
      .then((res) => isDevMode() && console.log("Connection started", res))
      .catch(err => console.error("Error while starting connection: " + err));

    this.hubConnection.on(ExpectedMessage.welcome, (response) => {
      isDevMode() && console.log(`Welcome: ${JSON.stringify(response)}`);
      this.connectionId = response.data.connectionId;
      this.connectionId$.next(this.connectionId);

      const payload = {
        userId: this.userId,
        username: this.username,
        connectionId: this.connectionId,
        pageUrl: this.encodeService.base64Url(this.actualUrl),
        pictureUrl: this.encodeService.base64Url(this.picture),
        mail: "alice@example.com",
        fullname: "Alice Fox",
        position: "Frontend Developer",
        role: "Reviewer"
      }

      this.notifyConnection(payload);

    });

    this.manageReceivedData()
    this.manageNotifications()
    this.manageValidationError()
  }

  private hubConnectionBuild() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}SignalHub?${this.queryParamUserId}=${this.userId}`)
      .build();
  }

  private notifyConnection(payload: any) {
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

  private manageNotifications() {
    this.hubConnection.on(ExpectedMessage.show_notification, () => {
      this.showNotification();
    });
  }

  private manageReceivedData() {
    this.hubConnection.on(ExpectedMessage.received_data, (data) => {
      isDevMode() && console.log({received_data: data})
    });
  }

  private manageValidationError() {
    this.hubConnection.on(ExpectedMessage.validation_error, (data) => {
      isDevMode() && console.log({validation_error: data})
    });
  }
}
