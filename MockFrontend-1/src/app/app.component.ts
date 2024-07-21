import { Component, OnInit } from '@angular/core';
import { SignalRService } from './signal-r.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit {

  title = 'SignalRPOC';
  connectionName = "alice";

  constructor(private signalRService: SignalRService) { }

  public ngOnInit() {
    this.signalRService.startConnection();
  }

  public sendPayload() {
    const payload = { message: 'Hello, SignalR!' };
    this.signalRService.sendPayload(payload);
  }
}
