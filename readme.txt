

                       i3lets PowerShell for Interactive Intelligence


-- Introduction --

		The i3lets PowerShell cmd-lets provide a means of interacting with an
		Interactive Intelligence based phone system. These cmd-lets were
		created based off the originally provided ConfigurationCmdlet but
		expand on it in several ways. First and foremost, this compiles
		with the more recent ICELib SDKs (2016r1) and does away
		with registering the now depricated SnapIn function of PowerShell 2.

		Furthermore, these cmd-lets have been renamed from the original example
		to feature the prefix of "i3" so as to avoid collision/call out the difference
		with other PowerShell modules in various environments

-- Module --

        The Module registers all cmdlets within the i3lets.dll
        library with PowerShell and allows them to be used inside a shell. The
		module (i3lets.dll) and other files should all be placed in a folder
		called "i3lets" and placed accordingly per the following examples.
		After this you can import them which can be done simply by using...

			PS> import-module "\\server\share\\i3lets\i3lets.dll"
				or
			PS> import-module "\\localFolder\i3lets\i3lets.dll"

        The dll can be located anywhere (local/network share) but must be colocated
		with requesite IceLib dlls. Once imported, you can begin using the cmd-lets.

		If you want to be able to natively perform...

			PS> import-module i3lets

		Then you need to move the i3lets folder into the PowerShell folder on your
		local PC/server located at...

			C:\Windows\System32\WindowsPowerShell\v1.0\Modules\i3lets\

-- Cmdlets --

        This PowerShell module currently provides 18 cmd-lets to administer
		users within the phone system

    -- New-i3Session --

        New-i3Session creates a session and connects that session
        to the Interaction Center Server specified. Parameters for
        New-Session are Host, UserName, and Password. The Host
        parameter is required but if either the UserName or Password is left
        out, the system will use Windows Authentication. If UserName and
        Password are specified, the system will use Interaction Center
        Authentication Settings.

        Example:
        New-Session icserver
        New-Session -Host icserver
        New-Session icserver username password
        New-Session -Host icserver -UserName user -Password pass

    -- Get-i3Session --

        Get-i3Session will get a list of active sessions. Get-Session has one 
        optional parameter, SessionId. If specified, Get-Session will return the 
        session the matching session id. Otherwise, all sessions will be 
        returned. 

        Example:
        Get-Session
        Get-Session 1415
        Get-Session -SessionId 1415

    -- Remove-i3Session --

        Remove-i3Session destroys a session with the server. It is important to 
        call this before exiting the PowerShell so the server will be notified. 
        It requires a session be specified or pipelined. By pipelining 
        Get-Session with no parameters to Remove-Session, all active sessions 
        will be disconnected. 

        Example:
        Remove-Session $session
        Get-Session | Remove-Session

    -- Get-i3User --

        Get-i3User searches the Interaction Center Server for a list of user
        configurations. All parameters for Get-User are optional and are used
        to filter the results of the search. The UserId parameter performs an
        exact match on the user's unique identifier. The DisplayName parameter
        searches for all users who's email display name contains the specified
        value. If no parameters are specified, a list of all users will be
        returned. Note: The dollar sign ($) character must be escaped by the
        tick (`) character for a literal dollar sign.

        Example:
        Get-User
        Get-User -UserId `$Jane.Smith
        Get-User -DisplayName Jane

    -- New-i3User --

        New-i3User will create a new configuration for the specified user. The two
        require parameters are GivenName and Surname. The these two parameters
        are used to form the new user's id. The other optional parameters are
        Extension, DefaultWorkstation, Roles, and Workgroups. If the
        DefaultWorkstation, one of the Roles, or one of the Workgroups specified
        is not already configured, the add operation will fail. Roles and
        Workgroups are comma separated lists.

        Example:
        New-User Jane Smith
        New-User Jane Smith 1234 JANESMITHSIP Administrator,Agent Marketing,Sales
        New-User -GivenName Jane -Surname Smith -Extension 1234 -DefaultWorkstation JANESMITHSIP -Roles Administrator -Workgroups ITSupport

	-- Set-i3User --

        Set-i3User will set the configuration for the specified user. The two
        require parameters are session and user id. This is effectively the
		same operation as a new user, but instead allows edits on users
		currently in the system

        Example:
        New-User Jane Smith
        New-User Jane Smith 1234 JANESMITHSIP Administrator,Agent Marketing,Sales
        New-User -GivenName Jane -Surname Smith -Extension 1234 -DefaultWorkstation JANESMITHSIP -Roles Administrator -Workgroups ITSupport

    -- Remove-i3User --

        Remove-User will permanently delete a user from the Interaction Center
        Server. A list (or single user) user objects must be supplied to remove users.
		The Get-i3User command returns a list of user objects. This command utilizes
        the pipelining feature of PowerShell to know what users to remove.

        Example:
        Get-User -UserId `$Jane.Smith | Remove-User

    -- Set-i3Password --

        Set-i3Password will configure the password for a user or a group of users.
        Like the Remove-i3User command it obtains its list of users from the
        pipeline. In addition to the list of users, Set-Password requires the
        NewPassword parameter. If Interaction Center Server has password
        policies in place, this command will fail if the supplied password does
        not meet those requirements. To force the Interaction Center Server to
        set the passwords ignoring these password policies, enable the Force
        switch.

        Example:
        Get-User -UserId `$Jane.Smith | Set-Password password2
        Get-User -UserId `$Jane.Smith | Set-Password -NewPassword password2
        Get-User -UserId `$Jane.Smith | Set-Password password2 -Force
		
		Example 2 (use get-random to generate passwords for automation platforms/help desk portals)
		$pass = get-random -Minimum 100000 -Maximum 999999
		get-i3user -Session $i3session -UserId `$Jane.Smith | Set-i3Password -NewPassword $pass -Force


	-- Add-i3WorkgroupMember --
		Add a user within the phone system to the specified workgroup

		Example:
		Add-i3WorkgroupMember -Workgroup "myWorkGroupName" -UserId "usernameToAdd"

	-- Remove-i3WorkgroupMember --
		Remove a user within the phone system to the specified workgroup

		Example:
		Remove-i3WorkgroupMember -Workgroup "myWorkGroupName" -UserId "usernameToAdd"

	-- Add-i3RoleMember --
		Add a user within the phone system to the specified role

		Example:
		Add-i3RoleMember -Workgroup "myRoleName" -UserId "usernameToAdd"

	-- Remove-i3RoleMember --
		Remove a i3RoleMember within the phone system to the specified role

		Example:
		Remove-i3RoleMember -Workgroup "myRoleName" -UserId "usernameToRemove"

-- Formatting --

        In addition to the cmdlets, this example shows how IceLib classes can be
        formatted in PowerShell. The formatting example file is loaded by
        default if you start PowerShell from the StartPowerShell.bat file. The
        format file is Configuration.Format.ps1xml and contains a definition for
        ConfigurationManager and two definitions for UserConfiguration (table
        and list). The table format for UserConfiguration is a good way to get
        the overview on many UserConfiguration objects but when more detail is
        required you can format the data as a list by piping the result to the
        Format-List cmdlet. See the Format-List and Format-Help cmdlets for more
        information on formatting.

        Example:
        Get-User -UserId `Jane.Smith | Format-List
        Get-User -UserId `Jane.Smith | Format-Table