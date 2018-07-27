import { HttpErrorResponse } from "@angular/common/http";

/**
 * 提供HTML 呼叫與顯示 Alert component
 */
export class AlertBaseComponent {
    /* for alert.component */
    public alertType: AlertType;
    public alertMessage: string;

    /* 標準訊息 */
    protected get MessageFormValidationFailed(): string { return "資料檢核失敗，無法存檔，請找一下紅色框框的輸入錯誤"; }
    protected get MessageSaveSuccess(): string { return "存檔成功！"; }

    showMessage(type: AlertType, message: string) {
        this.alertType = type;
        this.alertMessage = message;
    }

    showError(type: AlertType, error: any) {
        this.alertType = type;
        this.alertMessage = this.parseError(error);
    }

    showSaveSuccess() {
        this.alertType = AlertType.Success;
        this.alertMessage = this.MessageSaveSuccess;
    }

    showOk() {
        this.alertType = AlertType.Info;
        this.alertMessage = "資料已經檢查 OK";
    }

    parseError(error: HttpErrorResponse): string {
        if (error.error) {
            // for ModelState Error
            return JSON.stringify(error.error);

        } else if (error.message) {
            return error.message;
        } else if (error.statusText) {
            return error.statusText;
        }
        return JSON.stringify(error);
    }
}

export enum AlertType {
    Success = "success",
    Info = "info",
    Warning = "warning",
    Danger = "danger"
}