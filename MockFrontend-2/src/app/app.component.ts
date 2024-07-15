import { HttpHeaders } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit, OnDestroy {

  title = 'SignalRPOC';
  connectionName = "bob";

  private hubConnection: HubConnection;

  public ngOnInit() {

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`https://localhost:44385/SignalHub?connectionName=${this.connectionName}`)
      .build();

    this.hubConnection.start().then(() => {
      console.log("connection started");
    }).catch(err => console.error(err));

    this.hubConnection.onclose(() => {
      // debugger;
      setTimeout(() => {
        // debugger;
        this.hubConnection.start().then(() => {
          // debugger;
          console.log("connection started");
        }).catch(err => console.error(err));
      }, 5000);
    });

    this.hubConnection.on("clientMethodName", (data) => {
      // debugger;
      console.log(`clientMethodName: ${data}`);
      this.showNotification();
    });

    this.hubConnection.on("WelcomeMethodName", (data) => {
      // debugger;
      console.log(`WelcomeMethodName: ${data}`);
      this.hubConnection.invoke("GetDataFromClient", "user id", data).catch(err => console.error(err));
    });

  }

  ngOnDestroy(): void {
    this.hubConnection.start().then(() => {
      console.log("stopped");
    }).catch(err => console.log(err));
  }

  showNotification() {
      console.log('showNotification');
      var notification = new Notification(this.connectionName, {
        body : 'You clicked on the button !',
        icon : 'https://www.dondeir.com/wp-content/uploads/2020/06/legacy-la-serie-documentla-de-bob-marley-en-youtube.jpg'
      });

      setTimeout(function() { notification.close(); }, 5000);
  }

}
