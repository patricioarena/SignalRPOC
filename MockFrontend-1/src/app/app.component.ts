import { HttpHeaders } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { HubConnection } from '@microsoft/signalr';
import * as signalR from '@microsoft/signalr';
import { Usuario } from './Usuario';
import { SignalRService } from './services/signal-r/signal-r.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit {

  usuarios: Usuario[] = [
    { nombreUsuario: "jdoe", idUsuario: 1, email: "jdoe@example.com", cargo: "Desarrollador", imagenUsuario: "https://mighty.tools/mockmind-api/content/human/65.jpg" },
    { nombreUsuario: "asmith", idUsuario: 2, email: "asmith@example.com", cargo: "Diseñador", imagenUsuario: "https://mighty.tools/mockmind-api/content/human/44.jpg" },
    { nombreUsuario: "mjohnson", idUsuario: 3, email: "mjohnson@example.com", cargo: "Gerente", imagenUsuario: "https://mighty.tools/mockmind-api/content/human/41.jpg" },
    { nombreUsuario: "krodriguez", idUsuario: 4, email: "krodriguez@example.com", cargo: "Administrador", imagenUsuario: "https://mighty.tools/mockmind-api/content/human/46.jpg" },
    { nombreUsuario: "fbrown", idUsuario: 5, email: "fbrown@example.com", cargo: "Soporte Técnico", imagenUsuario: "https://mighty.tools/mockmind-api/content/human/37.jpg" },
    { nombreUsuario: "fbrown", idUsuario: 5, email: "fbrown@example.com", cargo: "Soporte Técnico", imagenUsuario: "https://mighty.tools/mockmind-api/content/human/37.jpg" },
    { nombreUsuario: "fbrown", idUsuario: 5, email: "fbrown@example.com", cargo: "Soporte Técnico", imagenUsuario: "https://mighty.tools/mockmind-api/content/human/37.jpg" },
    { nombreUsuario: "fbrown", idUsuario: 5, email: "fbrown@example.com", cargo: "Soporte Técnico", imagenUsuario: "https://mighty.tools/mockmind-api/content/human/37.jpg" },
    { nombreUsuario: "jdoe", idUsuario: 1, email: "jdoe@example.com", cargo: "Desarrollador", imagenUsuario: "https://mighty.tools/mockmind-api/content/human/65.jpg" },
    { nombreUsuario: "asmith", idUsuario: 2, email: "asmith@example.com", cargo: "Diseñador", imagenUsuario: "https://mighty.tools/mockmind-api/content/human/44.jpg" },
    { nombreUsuario: "mjohnson", idUsuario: 3, email: "mjohnson@example.com", cargo: "Gerente", imagenUsuario: "https://mighty.tools/mockmind-api/content/human/41.jpg" },
    { nombreUsuario: "krodriguez", idUsuario: 4, email: "krodriguez@example.com", cargo: "Administrador", imagenUsuario: "https://mighty.tools/mockmind-api/content/human/46.jpg" },

  ];

  title = 'SignalRPOC';
  username = "Alice";
  connectionId = "";

  constructor(private signalRService: SignalRService) { }

  public ngOnInit() {
    this.signalRService.startConnection();
    this.signalRService.connectionId$.subscribe((id: string) => {
      this.connectionId = id;
    })
  }

}
