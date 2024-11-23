import { HttpStatusCode } from "@angular/common/http";

export class ResponseMessage {
    message: string;
    description: string;

    constructor(message: string, description: string) {
        this.message = message;
        this.description = description;
    }
}

export class BaseVM {
    status: HttpStatusCode;
    messages: ResponseMessage[];

    constructor(code: HttpStatusCode = HttpStatusCode.Ok, ...messages: ResponseMessage[]) {
        this.status = code;
        this.messages = messages.length ? messages : [];
    }
}

export class BaseTVM<T> extends BaseVM {
    data: T | null;

    constructor(code: HttpStatusCode = HttpStatusCode.Ok, ...messages: ResponseMessage[]) {
        super(code, ...messages);
        this.data = null; // Initialize data as null
    }
}