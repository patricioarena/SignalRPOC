import { HttpHeaders } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { Usuario } from './Usuario';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit, OnDestroy {

  usuarios: Usuario[] = [
    { nombreUsuario: "jdoe", idUsuario: 1, email: "jdoe@example.com", cargo: "Desarrollador", imagenUsuario: "https://mighty.tools/mockmind-api/content/human/65.jpg" },
    { nombreUsuario: "asmith", idUsuario: 2, email: "asmith@example.com", cargo: "Diseñador", imagenUsuario: "https://mighty.tools/mockmind-api/content/human/44.jpg" },
    { nombreUsuario: "mjohnson", idUsuario: 3, email: "mjohnson@example.com", cargo: "Gerente", imagenUsuario: "https://mighty.tools/mockmind-api/content/human/41.jpg" },
    { nombreUsuario: "krodriguez", idUsuario: 4, email: "krodriguez@example.com", cargo: "Administrador", imagenUsuario: "https://mighty.tools/mockmind-api/content/human/46.jpg" },
    { nombreUsuario: "fbrown", idUsuario: 5, email: "fbrown@example.com", cargo: "Soporte Técnico", imagenUsuario: "https://mighty.tools/mockmind-api/content/human/37.jpg" },
    { nombreUsuario: "fbrown", idUsuario: 5, email: "fbrown@example.com", cargo: "Soporte Técnico", imagenUsuario: "https://mighty.tools/mockmind-api/content/human/37.jpg" },
    { nombreUsuario: "fbrown", idUsuario: 5, email: "fbrown@example.com", cargo: "Soporte Técnico", imagenUsuario: "https://mighty.tools/mockmind-api/content/human/37.jpg" },
    { nombreUsuario: "fbrown", idUsuario: 5, email: "fbrown@example.com", cargo: "Soporte Técnico", imagenUsuario: "https://mighty.tools/mockmind-api/content/human/37.jpg" }
  ];

  title = 'SignalRPOC';
  connectionName = "alice";

  private hubConnection: HubConnection;

  public ngOnInit() {

    this.usuarios.forEach(usuario => {
      console.log(usuario);
    });

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
    this.hubConnection.off("askServerResponse");
  }

  showNotification() {
      console.log('showNotification');
      var notification = new Notification(this.connectionName, {
        body : 'You clicked on the button !',
        icon : 'https://i.pinimg.com/originals/35/0e/dd/350edd537688ad50ea3c5615e02ba84e.jpg'
      });

      setTimeout(function() { notification.close(); }, 5000);
  }

}
