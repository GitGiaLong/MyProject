namespace Core.Functions.Services

module Auth =
    let checkPermission (viewerRole: string) (contentRole: string) : bool =
        match viewerRole, contentRole with
        | "Admin", _ -> true
        | "Family", "family" -> true
        | "Friend", "friends" | "Friend", "public" -> true
        | "User", "public" -> true
        | "Guest", "public" -> true
        | _ -> false

